package com.cognizant.blueprinttemplate.util;

import io.jsonwebtoken.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class JwtTokenUtil {

    private Logger LOGGER = LoggerFactory.getLogger(JwtTokenUtil.class);

    @Value("${plugins.api.key}")
    private String pluginsApiKey;

    /**
     * A method to get JWT claims from given token
     *
     * @param token String
     *
     * @return String
     */
    public Claims getJwtClaimsFromToken(String token) {
        try {
            return Jwts.parser()
                    .setSigningKey(pluginsApiKey)
                    .parseClaimsJws(token.replace("Bearer ", ""))
                    .getBody();
        } catch (Exception e) {
            LOGGER.error("Exception occurred while parsing the JWT token - "
                    + (e.getCause() != null ? e.getCause().toString() : e.getMessage()));
        }
        return null;
    }

}
