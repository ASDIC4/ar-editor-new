package com.example.service;

import com.example.dao.AssetDao;
import com.example.entity.Asset;
import com.example.entity.Params;
import com.github.pagehelper.PageHelper;
import com.github.pagehelper.PageInfo;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import java.util.List;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
@Service
public class AssetService {
    private static final Logger logger = LoggerFactory.getLogger(AssetService.class);

    @Resource
    private AssetDao assetDao;

    public List<Asset> findAll() {
        return assetDao.selectAll();
    }
    public PageInfo<Asset> getAllAssets() {
        List<Asset> list = assetDao.selectAll(); // 使用Mapper提供的方法获取所有模型

        return PageInfo.of(list);
    }
    public PageInfo<Asset> findBySearch(Params params){
        logger.info("接收到的查询参数：assetName={}, assetId={}", params.getAssetName(), params.getAssetId());

        PageHelper.startPage(params.getPageNum(),params.getPageSize());

        List<Asset> list = assetDao.findBySearch(params); // 使用Mapper提供的方法获取所有模型
        System.out.println(list);
        logger.info("查询到的数据条数：{}", list.size());
        return PageInfo.of(list);
    }
    public List<Asset> find(String userId){
        List<Asset> list = assetDao.find(userId);
        return list;
    }
    public List<Asset> planeSearchPictureVideo(Integer userId){
        List<Asset> list = assetDao.planeSearchPictureVideo(userId);
        return list;
    }

    public List<Asset> planeSearchModel(Integer userId){
        List<Asset> list = assetDao.planeSearchModel(userId);
        return list;
    }

//    public void addAsset(Asset asset){
//        assetDao.addAsset(asset); // 使用Mapper提供的方法根据ID删除模型
//    }

    public Asset getAssetById(Integer assetId) {
        return assetDao.getAssetById(assetId); // 使用Mapper提供的方法根据ID获取模型
    }

    public void insertAsset(Asset asset) {
        assetDao.insertAsset(asset); // 使用Mapper提供的方法插入模型数据
    }

    public void deleteAsset(Asset asset) {
        assetDao.deleteAsset(asset); // 使用Mapper提供的方法根据ID删除模型
    }
    public void updateAssetNameById(Integer assetId, String assetNewName) {
        assetDao.updateAssetNameById(assetId,assetNewName); // 使用Mapper提供的方法根据ID删除模型
    }
    public Integer getMaxAssetId(){
        return assetDao.getMaxAssetId();
    }
}
