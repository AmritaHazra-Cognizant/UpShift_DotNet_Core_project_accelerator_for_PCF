package com.cognizant.dotnetcoreprojectaccelerator.controller;

import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.AUTHORIZATION;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.ERROR;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.FAIL;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.PLUGIN_OUTPUT;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.PROCESS_INSTANCE_ID;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.PROCESS_INSTANCE_STATUS;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.STATUS;
import static com.cognizant.dotnetcoreprojectaccelerator.constants.Constants.UPSHIFT_ORCHESTRATION_PLUGINS_STATUS_ENDPOINT;

import java.io.File;
import java.io.FileOutputStream;
import java.nio.file.Files;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.io.FileUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.client.RestTemplate;

import com.cognizant.dotnetcoreprojectaccelerator.constants.Constants;
import com.cognizant.dotnetcoreprojectaccelerator.model.MigratorEntity;
import com.cognizant.dotnetcoreprojectaccelerator.model.Output;
import com.cognizant.dotnetcoreprojectaccelerator.service.ApplicationService;
import com.cognizant.dotnetcoreprojectaccelerator.service.CommonService;
import com.cognizant.dotnetcoreprojectaccelerator.service.OutputService;
import com.cognizant.dotnetcoreprojectaccelerator.service.persistence.FilePersistenceService;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.gson.JsonObject;

@Controller
@RequestMapping("/")
public class ApplicationController {

	private Logger LOGGER = LoggerFactory.getLogger(ApplicationController.class);

	@Value("${upshiftUrl:http://localhost:8088}")
	private String upshiftUrl;

	@Autowired
	private RestTemplate restTemplate;

	@Autowired
	private ApplicationService service;

	@Autowired
	private OutputService outputService;

	@Autowired
	private CommonService commonService;

	@Autowired
	private FilePersistenceService filePersistenceService;

	public void initiateResponseToUpShift(String processInstanceId, String applicationName) {
		LOGGER.info("Starting to send status message to UpShift (Orchestration Engine) for processInstanceId "
				+ processInstanceId);

		JsonObject responseBodyJson = new JsonObject();
		responseBodyJson.addProperty(PROCESS_INSTANCE_ID, processInstanceId);

		Output output = outputService.findOne(processInstanceId);
		String status = output.getStatus();

		JsonObject pluginoutput = new JsonObject();
		pluginoutput.addProperty("Process Instance ID", processInstanceId);
		pluginoutput.addProperty("Status", status);
		pluginoutput.addProperty("Date", LocalDate.now().toString());
		String fileName = applicationName + "-WEBAPI.zip";
		pluginoutput.addProperty("Output File Name", fileName);

		responseBodyJson.add(PLUGIN_OUTPUT, pluginoutput);
		responseBodyJson.addProperty(STATUS, status);
		if (FAIL.equalsIgnoreCase(status)) {
			responseBodyJson.addProperty(PROCESS_INSTANCE_STATUS, ERROR);
		}

		String authorizationString = commonService.getAuthorizationToken();
		HttpHeaders httpHeaders = new HttpHeaders();
		httpHeaders.add(AUTHORIZATION, authorizationString);

		String postStatusUrl = upshiftUrl.concat(UPSHIFT_ORCHESTRATION_PLUGINS_STATUS_ENDPOINT).concat("/")
				.concat(processInstanceId);
		HttpEntity<String> requestBody = new HttpEntity<>(responseBodyJson.toString(), httpHeaders);
		ResponseEntity<String> response = restTemplate.postForEntity(postStatusUrl, requestBody, String.class);

		LOGGER.info("Status message to UpShift (Orchestration Engine) has been sent. " + responseBodyJson);
	}

	@RequestMapping(path = "/result")
	public String getResult(HttpServletRequest request, Model model)
			throws JsonMappingException, JsonProcessingException {

		String processInstanceId = request.getParameter("processInstanceId");
		LOGGER.info("In result endpoint processInstanceId " + processInstanceId);

		Output output = outputService.findOne(processInstanceId);
		ObjectMapper mapper = new ObjectMapper();
		MigratorEntity entity = mapper.readValue(output.getEntityString(), MigratorEntity.class);
		List<String> commonFramework = new ArrayList<>();
		commonFramework.add("Logging");
		commonFramework.add("JWT");
		commonFramework.add("Exception Handling");
		commonFramework.add("Caching");
		commonFramework.add("SSO");
		// commonFramework.add("Health Check");
		commonFramework.add("Swagger Support");
		entity.setCommonFrameworks(commonFramework);
		model.addAttribute("entity", entity);
		return "result";
	}

	@RequestMapping(path = "/accelerator")
	public String getMigrator(HttpServletRequest request, Model model) {

		String processInstanceId = request.getParameter("processInstanceId");
		LOGGER.info("In accelerator endpoint processInstanceId " + processInstanceId);
		MigratorEntity entity = new MigratorEntity();
		List<String> commonFramework = new ArrayList<>();
		commonFramework.add("Logging");
		commonFramework.add("JWT");
		commonFramework.add("Exception Handling");
		commonFramework.add("Caching");
		commonFramework.add("SSO");
		// commonFramework.add("Health Check");
		commonFramework.add("Swagger Support");
		entity.setCommonFrameworks(commonFramework);
		entity.setProcessInstanceId(processInstanceId);
		model.addAttribute("entity", entity);
		return "migrator";
	}

	@RequestMapping(path = "/download", method = RequestMethod.POST)
	public void downloadFile(HttpServletRequest request, HttpServletResponse response, Model model) throws Exception {

		String processInstanceId = request.getParameter("processInstanceId");
		LOGGER.info("In download endpoint processInstanceId " + processInstanceId);

		Output output = outputService.findOne(processInstanceId);
		ObjectMapper mapper = new ObjectMapper();
		MigratorEntity entity = mapper.readValue(output.getEntityString(), MigratorEntity.class);

		LOGGER.info("In download - Entity Info " + entity.toString());

		List<String> commonFramework = new ArrayList<>();
		commonFramework.add("Logging");
		commonFramework.add("JWT");
		commonFramework.add("Exception Handling");
		commonFramework.add("Caching");
		commonFramework.add("SSO");
		// commonFramework.add("Health Check");
		commonFramework.add("Swagger Support");
		entity.setCommonFrameworks(commonFramework);
		model.addAttribute("entity", entity);

		String applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
		String folderName = entity.getApplnName().replaceAll("[^a-zA-Z0-9]", "_")+ "_WEBAPI_zip";
		//String folderName = StringUtils.replace(entity.getApplnName(), " ", "_") + "_WEBAPI_zip";
		
		String myHomePath = System.getProperty("user.home");
		String filePath = myHomePath + File.separator + "upshift" + File.separator + "plugins" + File.separator
				+ "objectStorage" + File.separator + "dotnetcoreprojectaccelerator" + File.separator + processInstanceId
				+ File.separator + folderName + File.separator + applicationName + "-WEBAPI.zip";

		byte[] data = Files.readAllBytes(new File(filePath).toPath());

		response.addHeader("Content-Disposition", "attachment; filename=\"" + applicationName + "-WEBAPI" + ".zip\"");
		response.setContentType("application/zip");
		response.getOutputStream().write(data);
		response.getOutputStream().flush();

		/*
		 * ResponseEntity responseEntity =
		 * filePersistenceService.getPluginDataFile(applicationName + "-WEBAPI.zip",
		 * entity.getProcessInstanceId()); File file = (File) responseEntity.getBody();
		 * byte[] zipout = FileUtils.readFileToByteArray(file);
		 */

	}

	@RequestMapping(path = "/createProject", method = RequestMethod.POST)
	public void getMigrator(HttpServletRequest request, HttpServletResponse response, MigratorEntity entity)
			throws Exception {

		LOGGER.info("Entity Info " + entity.toString());

		String processInstanceId = entity.getProcessInstanceId();
		String applicationName = null;
		File fileToBeStored = null;
		if (processInstanceId != null) {

			Output output = outputService.findOne(processInstanceId);
			if (output == null) {
				try {
					LOGGER.info("Initializing flow for processInstanceId " + processInstanceId);
					output = new Output(processInstanceId);
					output.setStatus(Constants.IN_PROGRESS);
					output = outputService.save(output);
					LOGGER.info("Initialized flow for processInstanceId " + processInstanceId);

					// Execute the plugin
					applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
					byte[] zipOutput = service.generateProject(entity);
					// String zipOutput = service.generateProject(entity);
					fileToBeStored = new File(applicationName + "-WEBAPI.zip");
					FileOutputStream fileOuputStream = new FileOutputStream(fileToBeStored);
					fileOuputStream.write(zipOutput);
					filePersistenceService.createPluginDataFile(applicationName + "-WEBAPI.zip", processInstanceId,
							fileToBeStored);
					fileOuputStream.close();
					System.out.println("Stored in DB");

					response.addHeader("Content-Disposition",
							"attachment; filename=\"" + applicationName + "-WEBAPI" + ".zip\"");
					response.setContentType("application/zip");
					response.getOutputStream().write(zipOutput);
					response.getOutputStream().flush();

					ObjectMapper mapper = new ObjectMapper();
					String entityStr = mapper.writeValueAsString(entity);

					output.setStatus(Constants.SUCCESS);
					output.setEntityString(entityStr);
				} catch (Exception e) {
					LOGGER.error("Exception while processing.", e);
					output.setStatus(FAIL);
				} finally {
					FileUtils.deleteQuietly(fileToBeStored);
				}
				outputService.save(output);

				// Inform UpShift that the process has been completed for this processInstanceId
				initiateResponseToUpShift(processInstanceId, applicationName);

				LOGGER.info("Finished processing for processInstanceId " + processInstanceId);
			} else {
				LOGGER.error("Flow for processInstanceId " + processInstanceId + " already initialized");
			}
		}
	}

}
