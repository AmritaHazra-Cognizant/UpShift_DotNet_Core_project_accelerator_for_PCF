package com.cognizant.blueprintwithui;

import com.cognizant.blueprintwithui.util.FileUtil;
import com.google.gson.JsonObject;
import org.apache.commons.io.IOUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.File;
import java.io.IOException;
import java.nio.file.Paths;
import java.util.Arrays;

import static com.cognizant.blueprintwithui.constants.Constants.*;

public class BlueprintWithUiApplication {

	public static void main(String[] args) throws Exception {
		BlueprintWithUiApplication blueprintWithUiApplication = new BlueprintWithUiApplication();
		blueprintWithUiApplication.run(args);
	}

	private Logger LOGGER = LoggerFactory.getLogger(BlueprintWithUiApplication.class);

	public void run(String... args) throws Exception {
		LOGGER.info(" Running with these args --> " + Arrays.toString(args));
		String localProjectPath = args[0];
		String pluginOutputFolderPath = args[1];
		LOGGER.info("localProjectPath - " + localProjectPath);
		LOGGER.info("pluginOutputFolderPath - " + pluginOutputFolderPath);

		// Execute the plugin
		executePlugin(localProjectPath, pluginOutputFolderPath);

		LOGGER.info("Finished executing custom recipe");
	}

	private void executePlugin(String localProjectPath, String pluginOutputFolderPath) throws IOException {
        JsonObject outputJson = new JsonObject();
		File outputFile = Paths.get(pluginOutputFolderPath, OUTPUT_JSON_FILE).toFile();

		try {
			PluginExecutor pluginExecutor = new PluginExecutor();
			// Execute the plugin
			pluginExecutor.execute(localProjectPath, pluginOutputFolderPath, outputJson);

            outputJson.addProperty("Status", "Success");
			FileUtil.writeToFile(outputFile, IOUtils.toInputStream(outputJson.toString(), UTF_8));
		} catch (Exception e) {
			LOGGER.error("Exception - ", e);
			String pluginErrorText = e.getMessage() != null ? e.getMessage() : e.toString();
			String pluginErrorMessage = e.getCause() != null ? e.getCause().toString() : e.getStackTrace()[0].toString();
            outputJson.addProperty("Status", "Failed");
            outputJson.addProperty("Error", pluginErrorText);
            outputJson.addProperty("Detailed Error", pluginErrorMessage);
			FileUtil.writeToFile(outputFile, IOUtils.toInputStream(outputJson.toString(), UTF_8));
			throw e;
		}
	}

}
