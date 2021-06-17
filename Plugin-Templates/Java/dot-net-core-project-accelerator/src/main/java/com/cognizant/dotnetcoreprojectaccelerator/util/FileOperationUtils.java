package com.cognizant.dotnetcoreprojectaccelerator.util;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.Set;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.IOUtils;
import org.springframework.core.io.Resource;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;
import org.springframework.core.io.support.ResourcePatternResolver;

public class FileOperationUtils {

	public static boolean createRootDirectory(String outputFolder) {

		if (new File(outputFolder).exists()) {
			new File(outputFolder).delete();
		}
		return new File(outputFolder).mkdir();

	}

	public static boolean createSubDirectory(String outputFolder) {
		return new File(outputFolder).mkdir();
	}

	public static void copyDirectory(String sourceDirectoryLocation, String destinationDirectoryLocation)
			throws IOException {
		File sourceDirectory = new File(sourceDirectoryLocation);
		File destinationDirectory = new File(destinationDirectoryLocation);
		FileUtils.copyDirectory(sourceDirectory, destinationDirectory);
	}

	public static void copyDirectoryContent(String sourceDirectoryLocation, String destinationDirectoryLocation)
			throws IOException {

		System.out.println(sourceDirectoryLocation);
		ResourcePatternResolver resolver = new PathMatchingResourcePatternResolver();
		org.springframework.core.io.Resource[] resources = resolver.getResources(sourceDirectoryLocation);
		File destinationDirectory = new File(destinationDirectoryLocation);
		if (resources.length > 0) {
			FileUtils.copyDirectory(resources[0].getFile(), destinationDirectory);
		}
	}

	public static void copyfilesInFolder(String source, String destination) throws IOException {

		ResourcePatternResolver resolver = new PathMatchingResourcePatternResolver();
		org.springframework.core.io.Resource[] resources = resolver.getResources(source);
		for (Resource resource : resources) {
			InputStream is = resource.getInputStream();
			try {
				FileUtils.copyInputStreamToFile(is, new File(destination + File.separator + resource.getFilename()));
			} finally {
				IOUtils.closeQuietly(is);
			}
		}
	}

	public static void listFilesInSubDirectory(String dir, Set<File> setOfFiles) {

		File directory = new File(dir);
		File[] listOfFiles = directory.listFiles();
		for (File file : listOfFiles) {
			if (file.isDirectory()) {
				listFilesInSubDirectory(file.getAbsolutePath(), setOfFiles);
			} else {
				setOfFiles.add(file.getAbsoluteFile());
			}
		}
	}
}
