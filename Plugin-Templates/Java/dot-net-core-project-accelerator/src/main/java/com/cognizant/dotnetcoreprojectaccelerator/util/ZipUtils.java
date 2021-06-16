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

}
