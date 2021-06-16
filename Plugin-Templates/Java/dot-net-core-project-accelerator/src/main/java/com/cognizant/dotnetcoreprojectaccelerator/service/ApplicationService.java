package com.cognizant.dotnetcoreprojectaccelerator.service;

import java.io.File;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.HashSet;
import java.util.Set;

import org.apache.commons.io.FileUtils;
import org.springframework.util.StringUtils;

import com.cognizant.dotnetcoreprojectaccelerator.model.MigratorEntity;
import com.cognizant.dotnetcoreprojectaccelerator.util.FileOperationUtils;
import com.cognizant.dotnetcoreprojectaccelerator.util.ZipUtils;

public class ApplicationService {

	public byte[] generateProject(MigratorEntity entity) throws IOException {

		String applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
		String outputFolder = applicationName + "-WEBAPI";
		byte[] zipout = null;
		String folderPath = null;
		try {

			FileOperationUtils.createRootDirectory(outputFolder);
			generateRootFolder(entity, outputFolder);

			// BusinessLogic
			folderPath = outputFolder + File.separator + applicationName + ".API.BusinessLogic";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.BusinessLogic");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.BusinessLogic.csproj",
					folderPath + File.separator + applicationName + ".API.BusinessLogic.csproj");

			// Caching
			folderPath = outputFolder + File.separator + applicationName + ".API.Caching";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Caching");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Caching.csproj",
					folderPath + File.separator + applicationName + ".API.Caching.csproj");

			// CommonUtils
			folderPath = outputFolder + File.separator + applicationName + ".API.CommonUtils";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.CommonUtils");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.CommonUtils.csproj",
					folderPath + File.separator + applicationName + ".API.CommonUtils.csproj");

			// Contract
			folderPath = outputFolder + File.separator + applicationName + ".API.Contract";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Contract");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Contract.csproj",
					folderPath + File.separator + applicationName + ".API.Contract.csproj");

			// DataService
			folderPath = outputFolder + File.separator + applicationName + ".API.DataService";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.DataService");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.DataService.csproj",
					folderPath + File.separator + applicationName + ".API.DataService.csproj");

			// DBAccess
			folderPath = outputFolder + File.separator + applicationName + ".API.DBAccess";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.DBAccess");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.DBAccess.csproj",
					folderPath + File.separator + applicationName + ".API.DBAccess.csproj");

			// Logging
			folderPath = outputFolder + File.separator + applicationName + ".API.Logging";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Logging");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Logging.csproj",
					folderPath + File.separator + applicationName + ".API.Logging.csproj");

			// Tests
			folderPath = outputFolder + File.separator + applicationName + ".API.Tests";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Tests");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Tests.csproj",
					folderPath + File.separator + applicationName + ".API.Tests.csproj");

			// Web
			folderPath = outputFolder + File.separator + applicationName + ".API.Web";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.API.Web");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.API.Web.csproj",
					folderPath + File.separator + applicationName + ".API.Web.csproj");

			// Framework
			folderPath = outputFolder + File.separator + applicationName + ".Framework";
			generateSubDirectories(entity, folderPath, "classpath*:dotnet-code-content/DotNetCore.Framework");
			renameCsProjFile(folderPath + File.separator + "DotNetCore.Framework.csproj",
					folderPath + File.separator + applicationName + ".Framework.csproj");

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
		if(file.getName().contains("csproj") || file.getName().contains("sln") ) {
			content = content.replaceAll("DotNetCore", applicationName);
		}else {
			content = content.replaceAll("DotNetCore", entity.getNameSpace());
		}
		
		FileUtils.write(file, content, StandardCharsets.UTF_8);
	}

	private void generateSubDirectories(MigratorEntity entity, String folderPath, String classpath) throws IOException {

		FileOperationUtils.createSubDirectory(folderPath);
		FileOperationUtils.copyDirectoryContent(classpath, folderPath);
		Set<File> setOfFiles = new HashSet<>();
		FileOperationUtils.listFilesInSubDirectory(folderPath, setOfFiles);
		for (File file : setOfFiles) {
			replaceContentInFile(entity, file);
		}

	}

	private void renameCsProjFile(String srcFilePath, String destFilePath) throws IOException {
		FileUtils.moveFile(new File(srcFilePath), new File(destFilePath));
	}
}

