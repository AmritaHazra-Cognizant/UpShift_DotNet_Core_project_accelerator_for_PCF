package com.cognizant.blueprinttemplate.dao;

import com.cognizant.blueprinttemplate.model.Output;
import com.cognizant.blueprinttemplate.repository.OutputRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.Optional;

@Component
public class OutputDaoImpl implements OutputDao {

    @Autowired
    private OutputRepository outputRepository;

    public Output save(Output output) {
        return outputRepository.save(output);
    }

    public List<Output> findAll() {
        return outputRepository.findAll();
    }

    public Output findOne(String id) {
        Optional<Output> outputOptional = outputRepository.findById(id);
        return outputOptional.orElse(null);
    }

    public void deleteById(String id) {
        outputRepository.deleteById(id);
    }


}
