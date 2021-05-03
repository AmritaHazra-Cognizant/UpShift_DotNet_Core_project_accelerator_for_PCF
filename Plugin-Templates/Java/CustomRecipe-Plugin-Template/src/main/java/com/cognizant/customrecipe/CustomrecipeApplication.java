package com.cognizant.customrecipe;

import com.cognizant.customrecipe.model.ErrorLog;
import com.cognizant.customrecipe.model.OperationDetail;
import com.cognizant.customrecipe.util.FileUtil;
import com.google.gson.JsonObject;
import org.apache.commons.io.IOUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.File;
import java.io.IOException;
import java.util.Arrays;

import static com.cognizant.customrecipe.constants.Constants.*;

public class CustomrecipeApplication {

	public static void main(String[] args) throws Exception {
		CustomrecipeApplication customrecipeApplication = new CustomrecipeApplication();
		customrecipeApplication.run(args);
	}

	private Logger LOGGER = LoggerFactory.getLogger(CustomrecipeApplication.class);


	public void run(String... args) throws Exception {
		LOGGER.info(" Running with these args --> " + Arrays.toString(args));
		String projectRootPath = args[0];
		String pathToOutputFile = args[1];
		LOGGER.info("projectRootPath - " + projectRootPath);
		LOGGER.info("pathToOutputFile - " + pathToOutputFile);
		File outputFile = new File(pathToOutputFile);

		executeCustomRecipe(projectRootPath, outputFile);

		LOGGER.info("Finished executing custom recipe");

	}

	private void executeCustomRecipe(String projectRootPath, File outputFile) throws IOException {

		OperationDetail operationDetail;
		try {
			RecipeExecutor recipeExecutor = new RecipeExecutor();
			// Execute the custom recipe
			recipeExecutor.execute(projectRootPath);

			operationDetail = new OperationDetail(CUSTOM_RECIPE_ID, CUSTOM_RECIPE_LABEL, SUCCESS);

		} catch (Exception e) {
			LOGGER.error("Exception - ", e);
			String customRecipeErrorText = e.getMessage() != null ? e.getMessage() : e.toString();
			String customRecipeErrorMessage = e.getCause() != null ? e.getCause().toString() : e.getStackTrace()[0].toString();
			ErrorLog errorLog = new ErrorLog(customRecipeErrorText, customRecipeErrorMessage);

			operationDetail = new OperationDetail(CUSTOM_RECIPE_ID, CUSTOM_RECIPE_LABEL, FAIL, errorLog);
		}

		writeOperationDetailsJsonToOutputFile(outputFile, operationDetail);
	}

	private void writeOperationDetailsJsonToOutputFile(File outputFile,
													   OperationDetail operationDetail
													   ) throws IOException {
		JsonObject responseJson = new JsonObject();
		responseJson.addProperty("operation", operationDetail.getOperation());
		responseJson.addProperty("label", operationDetail.getLabel());
		responseJson.addProperty("status", operationDetail.getStatus());
		ErrorLog errorLog = operationDetail.getErrorLog();
		if (errorLog != null) {
			JsonObject errorLogJson = new JsonObject();
			errorLogJson.addProperty("errorText", errorLog.getErrorText());
			errorLogJson.addProperty("errorMessage", errorLog.getErrorMessage());
			responseJson.add("errorLog", errorLogJson);
		}

		LOGGER.info(String.valueOf(responseJson));

		FileUtil.writeToFile(outputFile, IOUtils.toInputStream(responseJson.toString(), UTF_8));
	}

}
