package com.cognizant.blueprinttemplate.repository;

import com.cognizant.blueprinttemplate.model.Output;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface OutputRepository extends JpaRepository<Output, String> {


    List<Output> findAll();

    Optional<Output> findById(String id);

    Output save(Output settings);

    void deleteById(String id);

}
