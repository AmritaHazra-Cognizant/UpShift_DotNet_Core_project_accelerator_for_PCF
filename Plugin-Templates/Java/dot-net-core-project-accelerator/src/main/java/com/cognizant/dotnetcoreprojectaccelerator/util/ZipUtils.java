package com.cognizant.dotnetcoreprojectaccelerator.util;

import java.io.ByteArrayOutputStream;
import java.io.File;

import org.zeroturnaround.zip.ZipUtil;

public class ZipUtils {

	public static byte[] zipFolder(String outputFolder) {
		ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
		ZipUtil.pack(new File(outputFolder), byteArrayOutputStream);
		return byteArrayOutputStream.toByteArray();
	}
	
	public static String saveZipFolder(String outputFolder, String applnName) {
		
		String myHomePath= System.getProperty("user.home");
		String filePath = myHomePath+"/Downloads/" + applnName+"-WEBAPI.zip";
		ZipUtil.pack(new File(outputFolder), new File(filePath));
		return filePath;
	}

}
