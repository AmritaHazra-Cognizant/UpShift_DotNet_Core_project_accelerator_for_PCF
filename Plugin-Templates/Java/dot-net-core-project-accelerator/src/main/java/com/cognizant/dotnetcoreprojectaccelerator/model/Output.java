package com.cognizant.dotnetcoreprojectaccelerator.model;

import javax.persistence.Entity;
import javax.persistence.Id;

@Entity
public class Output {

	@Id
	private String processInstanceId;

	private String status;
	
	private String entityString;

	public Output() {
	}

	public Output(String processInstanceId) {
		this.processInstanceId = processInstanceId;
	}

	public Output(String processInstanceId, String projectName) {
		this.processInstanceId = processInstanceId;
	}

	public Output(String processInstanceId, String projectName, String status) {
		this.processInstanceId = processInstanceId;
		this.status = status;
	}

	// TODO: Can add more columns, if needed

	public String getProcessInstanceId() {
		return processInstanceId;
	}

	public void setProcessInstanceId(String processInstanceId) {
		this.processInstanceId = processInstanceId;
	}

	public String getStatus() {
		return status;
	}

	public void setStatus(String status) {
		this.status = status;
	}

	public String getEntityString() {
		return entityString;
	}

	public void setEntityString(String entityString) {
		this.entityString = entityString;
	}

	@Override
	public String toString() {
		return "Output [processInstanceId=" + processInstanceId + ", status=" + status + ", entityString="
				+ entityString + "]";
	}
}
