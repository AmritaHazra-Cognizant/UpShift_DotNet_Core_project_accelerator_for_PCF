package com.cognizant.dotnetcoreprojectaccelerator.model;

import java.util.List;

public class MigratorEntity {

	public String applnName;
	public String projectType;
	public String nameSpace;
	public String version;
	public String dbName;
	public String externalSrvcType;
	public String serverType;
	public String enableDockerSupport;
	public List<String> commonFrameworks;
	public String processInstanceId;

	public String getApplnName() {
		return applnName;
	}

	public void setApplnName(String applnName) {
		this.applnName = applnName;
	}

	public String getProjectType() {
		return projectType;
	}

	public void setProjectType(String projectType) {
		this.projectType = projectType;
	}

	public String getNameSpace() {
		return nameSpace;
	}

	public void setNameSpace(String nameSpace) {
		this.nameSpace = nameSpace;
	}

	public String getVersion() {
		return version;
	}

	public void setVersion(String version) {
		this.version = version;
	}

	public String getDbName() {
		return dbName;
	}

	public void setDbName(String dbName) {
		this.dbName = dbName;
	}

	public String getExternalSrvcType() {
		return externalSrvcType;
	}

	public void setExternalSrvcType(String externalSrvcType) {
		this.externalSrvcType = externalSrvcType;
	}

	public String getServerType() {
		return serverType;
	}

	public void setServerType(String serverType) {
		this.serverType = serverType;
	}

	public String getEnableDockerSupport() {
		return enableDockerSupport;
	}

	public void setEnableDockerSupport(String enableDockerSupport) {
		this.enableDockerSupport = enableDockerSupport;
	}

	public List<String> getCommonFrameworks() {
		return commonFrameworks;
	}

	public void setCommonFrameworks(List<String> commonFrameworks) {
		this.commonFrameworks = commonFrameworks;
	}

	public String getProcessInstanceId() {
		return processInstanceId;
	}

	public void setProcessInstanceId(String processInstanceId) {
		this.processInstanceId = processInstanceId;
	}

	@Override
	public String toString() {
		return "MigratorEntity [applnName=" + applnName + ", projectType=" + projectType + ", nameSpace=" + nameSpace
				+ ", version=" + version + ", dbName=" + dbName + ", externalSrvcType=" + externalSrvcType
				+ ", serverType=" + serverType + ", enableDockerSupport=" + enableDockerSupport + ", commonFrameworks="
				+ commonFrameworks + ", processInstanceId=" + processInstanceId + "]";
	}

}
