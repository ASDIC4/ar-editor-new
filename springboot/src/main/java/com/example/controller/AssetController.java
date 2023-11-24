// 编写 by 赵嘉诚
package com.example.controller;

import cn.hutool.core.io.FileUtil;
import com.example.common.Result;
import com.example.entity.Asset;
import com.example.entity.Params;
import com.example.service.AssetService;
import com.github.pagehelper.PageInfo;
import org.apache.tomcat.util.http.fileupload.FileUtils;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/asset")
public class AssetController {

    @Resource
    private AssetService assetService;

    @GetMapping
    public Result findAll() {
        List<Asset> list = assetService.findAll();
        return Result.success(list);
//        return userService.getUser();
    }
    @GetMapping("/search")
    public Result findBySearch(Params params){
        PageInfo<Asset> list = assetService.findBySearch(params);
        return Result.success(list);
    }
    @GetMapping("/search/app/{userId}")
    public List find(@PathVariable String userId){
        List<Asset> list = assetService.find(userId);
        return list;
    }
    @DeleteMapping("/delete/{assetId}")
    public Result delete(@PathVariable Integer assetId) {
        Asset myasset = assetService.getAssetById(assetId);
        assetService.deleteAsset(myasset);
        return Result.success();
    }
    @PostMapping("/edit/{assetId}/{assetNewName}")
    public Result update(@PathVariable Integer assetId,@PathVariable String assetNewName) {
        assetService.updateAssetNameById(assetId, assetNewName);
        return Result.success();
    }
//    @PostMapping("/add/{assetId}/{}")

    // 文件上传存储路径
    /**
     * 文件上传
     */
    private static final String USER_DIR = System.getProperty("user.dir");
    private static final String FILE_DIRECTORY = "examples";
    private static final String FILE_SEPARATOR = System.getProperty("file.separator");
    private static final String FILE_PATH = USER_DIR + FILE_SEPARATOR + FILE_DIRECTORY;

//    private static final String filePath = System.getProperty("user.dir") + "\\springboot\\static\\";
    @PostMapping("/upload")
    public Result upload(MultipartFile file) {
        synchronized (AssetController.class) {
            // 获取当前时间戳和原始文件名
//            Asset uploadAsset = new Asset();
//            uploadAsset.setAssetId(Integer.parseInt(params.getAssetId()));
//            uploadAsset.setUserId(Integer.parseInt(params.getUserId()));
//            uploadAsset.setAssetType(params.getAssetType());
//            uploadAsset.setAssetName(params.getAssetName());

            String flag = System.currentTimeMillis() + "";
            String fileName = file.getOriginalFilename();
            try {
                // 获取用户主目录
                // 应该是主目录级别
                // 拼接文件存储路径
                String filePath = FILE_PATH;

                // 如果没有目录，就会创建文件夹
                if (!FileUtil.isDirectory(filePath)) {
                    FileUtil.mkdir(filePath);
                }
                System.out.println("路径名");
                System.out.println(filePath);
                // 文件存储形式：时间戳-文件名
                FileUtil.writeBytes(file.getBytes(), filePath + flag + "-" + fileName);
            //    FileUtil.writeBytes(file.getBytes(),filePath + FILE_SEPARATOR + fileName);
                System.out.println(fileName + "--上传成功");
                Thread.sleep(1L);
//                assetService.insertAsset(uploadAsset);
            } catch (Exception e) {
                System.err.println(fileName + "--文件上传失败");
            }
            return Result.success(flag + "-" +fileName);
//            return Result.success(fileName);
        }
    }

    @DeleteMapping("/cancelUpload/{nowFileStoragePath}")
    public Result deleteUploadedFile(@PathVariable String nowFileStoragePath){
        try {
            FileUtils.forceDelete(new File(nowFileStoragePath));
            System.out.println("文件或目录删除成功");
        } catch (IOException e) {
            System.err.println("文件或目录删除失败: " + e.getMessage());
        }
        return Result.success();
    }

    @PostMapping("/confirmUpload/{assetName}/{assetType}/{userId}/{assetPath}")
    public Result confirmUpload(@PathVariable String assetName, @PathVariable String assetType, @PathVariable String assetPath,@PathVariable Integer userId){
        synchronized (AssetController.class) {
            Asset uploadAsset = new Asset(assetService.getMaxAssetId()+1,userId,assetName,assetPath,assetType);
            assetService.insertAsset(uploadAsset);
            System.out.println("看看Path");
            System.out.println(assetPath);
            return Result.success();
        }
    }

    @GetMapping("/{assetPath}")
    public ResponseEntity<byte[]> getStaticFile(@PathVariable String assetPath) throws IOException {
//        // Assuming static files are in the "static" directory
//        Path filePath = Paths.get(STATIC_FOLDER, assetPath);
////      Path filePath = new ClassPathResource("/static/" + assetPath).getFile().toPath();.getFile().toPath();
//
//        System.out.println("文件发送的filePath："+filePath);
//        // Read the file content into a byte array
//        byte[] fileContent = Files.readAllBytes(filePath);
//
//        // Determine the media type of the file based on the filename
//        MediaType mediaType = MediaType.parseMediaType(
//                Files.probeContentType(filePath));
//
//        // Build the response with file content and media type
//        return ResponseEntity.ok()
//                .contentType(mediaType)
//                .body(fileContent);
        try {
            // Assuming static files are in the "static" directory
            Path filePath = Paths.get(FILE_PATH , assetPath);

            // Check if the file exists
            if (Files.exists(filePath)) {
                // Read the file content into a byte array
                byte[] fileContent = Files.readAllBytes(filePath);

//                // Determine the media type of the file based on the filename
//                MediaType mediaType = MediaType.parseMediaType(
//                        Files.probeContentType(filePath));

                String mimeType = Files.probeContentType(filePath);
                if (mimeType == null) {
                    // If probeContentType returns null, use a default media type
                    mimeType = "application/octet-stream";
                }
                // Parse the media type
                MediaType mediaType = MediaType.parseMediaType(mimeType);

                // Build the response with file content and media type
                return ResponseEntity.ok()
                        .contentType(mediaType)
                        .body(fileContent);
            } else {
                // If the file does not exist, return an appropriate response
                return ResponseEntity.notFound().build();
            }
        } catch (IOException e) {
            System.out.println("我倒要看看谁在撒野");
            // Handle IOException, for example, log the error
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).build();
        }
    }
    @GetMapping("/search/app/pv/{userId}")
    public List<Asset> planeSearchPictureVideo(@PathVariable Integer userId){
        List<Asset> list = assetService.planeSearchPictureVideo(userId);
        return list;
    }
    @GetMapping("/search/app/model/{userId}")
    public List<Asset> planeSearchModel(@PathVariable Integer userId){
        List<Asset> list = assetService.planeSearchModel(userId);
        return list;
    }
//    @PostMapping("/upload/scene/app/")
//    public Result(){
//
//    }
//    @PostMapping("/upload/scene/app/")
//    public Result(){
//
//    }
}