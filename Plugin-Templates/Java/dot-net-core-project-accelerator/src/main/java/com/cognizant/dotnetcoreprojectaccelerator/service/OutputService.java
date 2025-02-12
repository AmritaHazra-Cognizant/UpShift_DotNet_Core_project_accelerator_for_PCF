package com.cognizant.dotnetcoreprojectaccelerator.service;

import com.cognizant.dotnetcoreprojectaccelerator.dao.OutputDaoImpl;
import com.cognizant.dotnetcoreprojectaccelerator.model.Output;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class OutputService {

    @Autowired
    OutputDaoImpl outputDao;

    public Output save(Output output) {
        return outputDao.save(output);
    }

    public List<Output> findAll() {
        return outputDao.findAll();
    }

    public Output findOne(String id) {
        return outputDao.findOne(id);
    }

    public void deleteById(String id) {
        outputDao.deleteById(id);
    }

}
