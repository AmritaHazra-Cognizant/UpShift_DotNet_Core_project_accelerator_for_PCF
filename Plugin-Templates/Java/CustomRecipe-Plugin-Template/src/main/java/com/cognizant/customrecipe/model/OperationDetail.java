package com.cognizant.customrecipe.model;

import java.io.Serializable;
import java.util.Objects;

public class OperationDetail implements Serializable {
	
	private String operation;
	
	private String label;
	
	private String status;

	private ErrorLog errorLog;

	public String getOperation() {
		return operation;
	}

	public void setOperation(String operation) {
		// Set Recipe ID as the operation
		this.operation = operation;
	}

	public String getLabel() {
		return label;
	}

	public void setLabel(String label) {
		this.label = label;
	}

	public String getStatus() {
		return status;
	}

	public void setStatus(String status) {
		this.status = status;
	}

	public ErrorLog getErrorLog() {
		return errorLog;
	}

	public void setErrorLog(ErrorLog errorLog) {
		this.errorLog = errorLog;
	}

	public OperationDetail() {
	}

	public OperationDetail(String operation, String label, String status) {
		this.operation = operation;
		this.label = label;
		this.status = status;
	}

	public OperationDetail(String operation, String label, String status, ErrorLog errorLog) {
		this.operation = operation;
		this.label = label;
		this.status = status;
		this.errorLog = errorLog;
	}

	@Override
	public boolean equals(Object o) {
		if (this == o) return true;
		if (o == null || getClass() != o.getClass()) return false;
		OperationDetail that = (OperationDetail) o;
		return Objects.equals(operation, that.operation) &&
				Objects.equals(label, that.label) &&
				Objects.equals(status, that.status) &&
				Objects.equals(errorLog, that.errorLog);
	}

	@Override
	public int hashCode() {

		return Objects.hash(operation, label, status, errorLog);
	}

	@Override
	public String toString() {
		return "OperationDetail{" +
				"operation='" + operation + '\'' +
				", label='" + label + '\'' +
				", status='" + status + '\'' +
				", errorLog=" + errorLog +
				'}';
	}
}
