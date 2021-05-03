package com.cognizant.blueprinttemplate.controller;

import com.cognizant.blueprinttemplate.model.Output;
import com.cognizant.blueprinttemplate.service.CommonService;
import com.cognizant.blueprinttemplate.service.OutputService;
import com.cognizant.blueprinttemplate.service.PluginExecutor;
import com.cognizant.blueprinttemplate.util.JwtTokenUtil;
import com.google.gson.JsonObject;
import io.jsonwebtoken.Claims;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.client.RestTemplate;

import static com.cognizant.blueprinttemplate.constants.Constants.*;

@RestController
@CrossOrigin
public class PluginController {

    private Logger LOGGER = LoggerFactory.getLogger(PluginController.class);

    @Value("${upshiftUrl:http://localhost:8088}")
    private String upshiftUrl;

    @Autowired
    private RestTemplate restTemplate;

    @Autowired
    private OutputService outputService;

    @Autowired
    private CommonService commonService;

    @Autowired
    private JwtTokenUtil jwtTokenUtil;

    @Autowired
    private PluginExecutor pluginExecutor;

    /**
     * This method is used to do some process for a particular processInstanceId
     *
     * @param processInstanceId String
     */
    @PostMapping("process/{processInstanceId}")
    public void processPlugin(@PathVariable("processInstanceId") String processInstanceId,
                              @RequestParam(value = "projectName", required = false) String projectName,
                              @RequestParam(value = "jwt", required = false) String jwtToken,
                              @RequestBody String requestBody) {

        // TODO: Get more inputs from the UI as required

        LOGGER.info("Processing for processInstanceId " + processInstanceId);

        Output output = outputService.findOne(processInstanceId);
        if (output == null) {
            try {
                LOGGER.info("Initializing flow for processInstanceId " + processInstanceId);
                output = new Output(processInstanceId, projectName);
                output.setStatus(IN_PROGRESS);
                output = outputService.save(output);
                LOGGER.info("Initialized flow for processInstanceId " + processInstanceId);

                if (jwtToken != null) {
                    Claims claims = jwtTokenUtil.getJwtClaimsFromToken(jwtToken);
                    if (claims != null) {
                        String userEmailId = String.valueOf(claims.get(USER_EMAIL_ID));
                        String userRole = String.valueOf(claims.get(USER_ROLE));
                        // TODO: Use userEmailId and userRole, as needed
                    }
                }

                // Execute the plugin
                pluginExecutor.execute(output);

                output.setStatus(SUCCESS);
            } catch (Exception e) {
                LOGGER.error("Exception while processing.", e);
                output.setStatus(FAIL);
            }
            outputService.save(output);

            // Inform UpShift that the process has been completed for this processInstanceId
            initiateResponseToUpShift(processInstanceId);

            LOGGER.info("Finished processing for processInstanceId " + processInstanceId);
        } else {
            LOGGER.error("Flow for processInstanceId " + processInstanceId + " already initialized");
        }
    }

    /**
     * This method is used to get the current status for a particular processInstanceId
     * (Can be used to know which page the plugin should currently be in)
     *
     * @param processInstanceId String
     */
    @GetMapping("status/{processInstanceId}")
    public String getStatusForGivenProcessInstanceId(@PathVariable("processInstanceId") String processInstanceId) {
        Output output = outputService.findOne(processInstanceId);
        return output == null ? "NotStarted" : output.getStatus();
    }

    /**
     * This method is used to get the output for a particular processInstanceId
     * (Can be used to display in results page (iFrame), if needed)
     *
     * @param processInstanceId String
     */
    @GetMapping("output/{processInstanceId}")
    public ResponseEntity<String> getOutputForGivenProcessInstanceId(@PathVariable("processInstanceId") String processInstanceId) {
        LOGGER.info("Fetching output for processInstanceId " + processInstanceId);
        JsonObject responseBodyJson = new JsonObject();
        responseBodyJson.addProperty(PROCESS_INSTANCE_ID, processInstanceId);

        Output output = outputService.findOne(processInstanceId);
        if (output == null) {
            return new ResponseEntity<>("Given processInstanceId is not active", HttpStatus.NOT_FOUND);
        }

        String status = output.getStatus();

        // TODO: Can add more values here, as needed

        responseBodyJson.addProperty(STATUS, status);
        return new ResponseEntity<String>(String.valueOf(responseBodyJson), HttpStatus.OK);
    }

    /**
     * This method is used to invoke status call to UpShift (Orchestration Engine)
     * (Mentioning that the flow is finished for this processInstanceId, with a status success or fail)
     *
     * @param processInstanceId - String
     */
    @PostMapping("postResultStatusToUpShift")
    public void initiateResponseToUpShift(@PathVariable("processInstanceId") String processInstanceId) {
        LOGGER.info("Starting to send status message to UpShift (Orchestration Engine) for processInstanceId " + processInstanceId);

        JsonObject responseBodyJson = new JsonObject();
        responseBodyJson.addProperty(PROCESS_INSTANCE_ID, processInstanceId);

        Output output = outputService.findOne(processInstanceId);
        String status = output.getStatus();

        JsonObject pluginoutput = new JsonObject();
        pluginoutput.addProperty("key", "value");
        // TODO: Edit/Add more values in pluginOutput object, as needed

        responseBodyJson.add(PLUGIN_OUTPUT, pluginoutput);
        responseBodyJson.addProperty(STATUS, status);
        if (FAIL.equalsIgnoreCase(status)) {
            responseBodyJson.addProperty(PROCESS_INSTANCE_STATUS, ERROR);
        }

        String authorizationString = commonService.getAuthorizationToken();
        HttpHeaders httpHeaders = new HttpHeaders();
        httpHeaders.add(AUTHORIZATION, authorizationString);

        String postStatusUrl = upshiftUrl
                .concat(UPSHIFT_ORCHESTRATION_PLUGINS_STATUS_ENDPOINT)
                .concat("/")
                .concat(processInstanceId);
        HttpEntity<String> requestBody = new HttpEntity<>(responseBodyJson.toString(), httpHeaders);
        ResponseEntity<String> response = restTemplate.postForEntity( postStatusUrl, requestBody, String.class);

        LOGGER.info("Status message to UpShift (Orchestration Engine) has been sent. " + responseBodyJson);
    }

}
