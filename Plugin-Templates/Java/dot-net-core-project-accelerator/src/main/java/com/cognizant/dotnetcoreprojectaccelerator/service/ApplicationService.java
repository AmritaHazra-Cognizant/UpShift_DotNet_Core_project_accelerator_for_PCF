package com.cognizant.dotnetcoreprojectaccelerator.service;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.charset.StandardCharsets;
import java.util.HashSet;
import java.util.Set;

import org.apache.commons.io.FileUtils;
import org.springframework.stereotype.Service;
import org.springframework.util.StringUtils;

import com.cognizant.dotnetcoreprojectaccelerator.model.MigratorEntity;
import com.cognizant.dotnetcoreprojectaccelerator.util.FileOperationUtils;
import com.cognizant.dotnetcoreprojectaccelerator.util.ZipUtils;

@Service
public class ApplicationService {

	public byte[] generateProject(MigratorEntity entity) throws Exception {

		String applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
		String outputFolder = applicationName + "-WEBAPI";
		byte[] zipout = null;
		String folderPath = null;
		try {

			FileOperationUtils.createRootDirectory(outputFolder);
			generateRootFolder(entity, outputFolder);

			// BusinessLogic
			folderPath = outputFolder + File.separator + applicationName + ".API.BusinessLogic";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.BusinessLogic/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.BusinessLogic.csproj",
					folderPath + File.separator + applicationName + ".API.BusinessLogic.csproj");

			folderPath = outputFolder + File.separator + applicationName + ".API.BusinessLogic" + File.separator
					+ "Contracts";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.BusinessLogic/Contracts/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.BusinessLogic" + File.separator
					+ "Implementations";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.BusinessLogic/Implementations/*");
			
			// Caching
			folderPath = outputFolder + File.separator + applicationName + ".API.Caching";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Caching/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Caching.csproj",
					folderPath + File.separator + applicationName + ".API.Caching.csproj");

			folderPath = outputFolder + File.separator + applicationName + ".API.Caching" + File.separator
					+ "MemoryCache";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Caching/MemoryCache/*");

			// CommonUtils
			folderPath = outputFolder + File.separator + applicationName + ".API.CommonUtils";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.CommonUtils/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.CommonUtils.csproj",
					folderPath + File.separator + applicationName + ".API.CommonUtils.csproj");

			// Contract
			folderPath = outputFolder + File.separator + applicationName + ".API.Contract";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Contract/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Contract.csproj",
					folderPath + File.separator + applicationName + ".API.Contract.csproj");

			folderPath = outputFolder + File.separator + applicationName + ".API.Contract" + File.separator
					+ "ResponseModels";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Contract/ResponseModels/*");

			// DataService
			folderPath = outputFolder + File.separator + applicationName + ".API.DataService";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.DataService/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.DataService.csproj",
					folderPath + File.separator + applicationName + ".API.DataService.csproj");

			folderPath = outputFolder + File.separator + applicationName + ".API.DataService" + File.separator
					+ "Contracts";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DataService/Contracts/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.DataService" + File.separator
					+ "Implementations";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DataService/Implementations/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.DataService" + File.separator
					+ "Providers" + File.separator + "Api";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DataService/Providers/Api/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.DataService" + File.separator
					+ "Providers" + File.separator + "Models";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DataService/Providers/Model/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.DataService" + File.separator
					+ "Proxy";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DataService/Proxy/*");

			// DBAccess
			folderPath = outputFolder + File.separator + applicationName + ".API.DBAccess";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.DBAccess/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.DBAccess.csproj",
					folderPath + File.separator + applicationName + ".API.DBAccess.csproj");

			folderPath = outputFolder + File.separator + applicationName + ".API.DBAccess" + File.separator
					+ "Contracts";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DBAccess/Contracts/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.DBAccess" + File.separator
					+ "Implementations";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.DBAccess/Implementations/*");

			

			// Logging
			folderPath = outputFolder + File.separator + applicationName + ".API.Logging";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Logging/*");

			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Logging.csproj",
					folderPath + File.separator + applicationName + ".API.Logging.csproj");

			// Tests
			folderPath = outputFolder + File.separator + applicationName + ".API.Tests";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Tests/*");

			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Tests.csproj",
					folderPath + File.separator + applicationName + ".API.Tests.csproj");

			// Web
			folderPath = outputFolder + File.separator + applicationName + ".API.Web";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Web/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Web.csproj",
					folderPath + File.separator + applicationName + ".API.Web.csproj");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator
					+ "ActionFilters";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/ActionFilters/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Controllers";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Controllers/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Extensions";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Extensions/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Middleware";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Middleware/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Properties";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Properties/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Security"
					+ File.separator + "Configuration";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Security/Configuration/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Security"
					+ File.separator + "Implementation";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Security/Implementation/*");

			folderPath = outputFolder + File.separator + applicationName + ".API.Web" + File.separator + "Security"
					+ File.separator + "Service";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.API.Web/Security/Service/*");

			

			// Framework
			folderPath = outputFolder + File.separator + applicationName + ".Framework";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.Framework/*");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.Framework.csproj",
					folderPath + File.separator + applicationName + ".Framework.csproj");


			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator + "Caching"
					+ File.separator + "CoreCaching";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Caching/CoreCaching/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "ExceptionHandling" + File.separator + "Models";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/ExceptionHandling/Models/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Attributes";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Attributes/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Configuration";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Configuration/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Exceptions";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Exceptions/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Extensions";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Extensions/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Interfaces";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Interfaces/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Internal" + File.separator + "Configuration";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Internal/Configuration/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Internal" + File.separator + "Extensions";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Internal/Extensions/*");

			folderPath = outputFolder + File.separator + applicationName + ".Framework" + File.separator
					+ "Interception" + File.separator + "Internal" + File.separator + "Interfaces";
			generateSubDirectories(entity, folderPath,
					"classpath*:dotnet-code-content/DotNetCore.Framework/Interception/Internal/Interfaces/*");

			
			zipout = ZipUtils.zipFolder(outputFolder);
		} catch (Exception ex) {
			throw ex;
		} finally {
			FileUtils.deleteDirectory(new File(outputFolder));
		}

		return zipout;

	}

	private void generateRootFolder(MigratorEntity entity, String outputFolder) throws IOException {

		String applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
		FileOperationUtils.copyfilesInFolder("classpath*:dotnet-code-content/sln/*", outputFolder);
		FileUtils.moveFile(new File(outputFolder + File.separator + "DotNetCore.WebAPI.Template.sln"),
				new File(outputFolder + File.separator + applicationName + ".WebAPI.Template.sln"));
		FileUtils.moveFile(new File(outputFolder + File.separator + "gitignore.txt"),
				new File(outputFolder + File.separator + ".gitignore"));

		File slnFile = new File(outputFolder + File.separator + applicationName + ".WebAPI.Template.sln");
		replaceContentInFile(entity, slnFile);
		File gitIgnoreFile = new File(outputFolder + File.separator + ".gitignore");
		replaceContentInFile(entity, gitIgnoreFile);
	}

	private void replaceContentInFile(MigratorEntity entity, File file) throws IOException {
		String content = FileUtils.readFileToString(file, StandardCharsets.UTF_8);
		String applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
		if (file.getName().contains("csproj") || file.getName().contains("sln")) {
			content = content.replaceAll("DotNetCore", applicationName);
		} else {
			content = content.replaceAll("DotNetCore", entity.getNameSpace());
		}

		FileUtils.write(file, content, StandardCharsets.UTF_8);
	}

	private void generateSubDirectories(MigratorEntity entity, String folderPath, String classpath)
			throws IOException, URISyntaxException {

		FileOperationUtils.createSubDirectory(folderPath);
		FileOperationUtils.copyfilesInFolder(classpath, folderPath);
		Set<File> setOfFiles = new HashSet<>();
		FileOperationUtils.listFilesInSubDirectory(folderPath, setOfFiles);
		for (File file : setOfFiles) {
			replaceContentInFile(entity, file);
		}

	}

	private void renameCsProjFile(String srcFilePath, String destFilePath) throws IOException {
		/*
		 * System.out.println("destFilePath >>" + destFilePath); FileUtils.moveFile(new
		 * File(srcFilePath), new File(destFilePath));
		 */
		new File(srcFilePath).renameTo(new File(destFilePath));
	}
}
