package com.cognizant.customrecipe.util;

import java.io.*;

public class FileUtil {

    /**
     * This static method will write the input stream to the given file
     *
     * @param file - File
     * @param inputStream - InputStream
     */
    public static void writeToFile(File file, InputStream inputStream) throws IOException {
        fileUpdate(file, inputStream, false);
    }

    /**
     * This static method will append the input stream to the given file
     *
     * @param file - File
     * @param inputStream - InputStream
     */
    public static void appendToFile(File file, InputStream inputStream) throws IOException {
        fileUpdate(file, inputStream, true);
    }

    /**
     * This static method will write/append the input stream to the given file
     *
     * @param file - File
     * @param inputStream - InputStream
     * @param append - boolean
     * @throws IOException
     */
    private static void fileUpdate(File file, InputStream inputStream, boolean append) throws IOException {
        InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
        BufferedReader in = new BufferedReader(inputStreamReader);
        FileWriter fstream = new FileWriter(file, append);
        BufferedWriter out = new BufferedWriter(fstream);
        String aLine;
        try {
            int lineNo = 1;
            while ((aLine = in.readLine()) != null) {
                //Process each line and add output to Dest.txt file
                if (lineNo > 1) {
                    out.newLine();
                }
                out.write(aLine);
                lineNo++;
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            out.close();
            in.close();
            fstream.close();
            inputStreamReader.close();
        }
    }

}
