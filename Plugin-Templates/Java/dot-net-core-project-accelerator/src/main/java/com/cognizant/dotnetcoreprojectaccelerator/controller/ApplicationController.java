package com.cognizant.dotnetcoreprojectaccelerator.controller;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

import com.cognizant.dotnetcoreprojectaccelerator.model.MigratorEntity;
import com.cognizant.dotnetcoreprojectaccelerator.service.ApplicationService;

@Controller
@RequestMapping("/")
public class ApplicationController {
	
	@Autowired
	private ApplicationService service;

	@RequestMapping(path = "/accelerator")
	public String getMigrator(Model model) {
		MigratorEntity entity = new MigratorEntity();
		List<String> commonFramework = new ArrayList<>();
		commonFramework.add("Logging");
		commonFramework.add("JWT");
		commonFramework.add("Exception Handling");
		commonFramework.add("Caching");
		commonFramework.add("SSO");
		//commonFramework.add("Health Check");
		commonFramework.add("Swagger Support");
		entity.setCommonFrameworks(commonFramework);
		model.addAttribute("entity", entity);
		return "migrator";
	}

	@RequestMapping(path = "/createProject", method = RequestMethod.POST)
	public void getMigrator(HttpServletRequest request, HttpServletResponse response, MigratorEntity entity)
			throws IOException {

		System.out.println(entity.toString());

		String applicationName = StringUtils.replace(entity.getApplnName(), " ", "-");
		byte[] output = service.generateProject(entity);
		response.addHeader("Content-Disposition",
				"attachment; filename=\"" + applicationName + "-WEBAPI" + ".zip\"");
		response.setContentType("application/zip");
		response.getOutputStream().write(output);
		response.getOutputStream().flush();
	}

}
