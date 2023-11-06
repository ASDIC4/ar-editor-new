package com.example.controller;

import cn.hutool.core.io.FileUtil;
import com.example.common.Result;
import com.example.entity.Asset;
import com.example.entity.Params;
import com.example.service.AssetService;
import com.github.pagehelper.PageInfo;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.annotation.Resource;
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
    @DeleteMapping("/{assetId}")
    public Result delete(@PathVariable Integer assetId) {
        assetService.deleteAssetById(assetId);
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
    private static final String filePath = System.getProperty("user.dir") + "/assetFiles/";
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
                // 如果没有目录，就会创建文件夹
                if (!FileUtil.isDirectory(filePath)) {
                    FileUtil.mkdir(filePath);
                }
                System.out.println("路径名");
                System.out.println(filePath);
                // 文件存储形式：时间戳-文件名
                FileUtil.writeBytes(file.getBytes(), filePath + flag + "-" + fileName);
                System.out.println(fileName + "--上传成功");
                Thread.sleep(1L);
//                assetService.insertAsset(uploadAsset);
            } catch (Exception e) {
                System.err.println(fileName + "--文件上传失败");
            }
            return Result.success(flag);
        }
    }
}