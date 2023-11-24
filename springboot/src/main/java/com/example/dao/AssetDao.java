// 编写 by 赵嘉诚
package com.example.dao;

import com.example.entity.Asset;
import com.example.entity.Params;
import com.example.entity.User;
import org.apache.ibatis.annotations.*;
import org.springframework.stereotype.Repository;
import tk.mybatis.mapper.common.Mapper;

import java.util.List;

@Repository
public interface AssetDao extends Mapper<Asset> {

    // 1. 基于注解的方式
    //@Select("select * from user")

    List<Asset> findBySearch(@Param("params")Params params);

    void deleteAsset(@Param("asset") Asset asset);

//    @Update("update assets set assetName = #{assetNewName} where assetId = #{assetId}")
//    void updateAssetNameById(@Param("assetId") Integer assetId, @Param("assetNewName") String assetNewName);
    @Update("update assets set assetName = #{assetNewName,jdbcType=VARCHAR} where assetId = #{assetId,jdbcType=INTEGER}")
    void updateAssetNameById(@Param("assetId") Integer assetId, @Param("assetNewName") String assetNewName);

    default void addAsset(Asset asset){}

    @Select("SELECT MAX(assetId) FROM assets")
    Integer getMaxAssetId();

    @Insert("INSERT INTO assets (assetName, assetId, assetPath, assetType, userId) " +
            "VALUES (#{asset.assetName}, #{asset.assetId}, #{asset.assetPath}, #{asset.assetType}, #{asset.userId})")
    @Options(useGeneratedKeys = true, keyProperty = "asset.assetId", keyColumn = "assetId")
    void insertAsset(@Param("asset") Asset asset);

    @Select("SELECT * from assets where userId = #{userId}")
    List<Asset> find(@Param("userId")String userId);

    @Select("SELECT * from assets where userId = #{userId} and assetType in ('picture','video')")
    List<Asset> planeSearchPictureVideo(@Param("userId")Integer userId);

    @Select("SELECT * from assets where userId = #{userId} and assetType = 'model' ")
    List<Asset> planeSearchModel(@Param("userId")Integer userId);

    @Select("SELECT * from assets where assetId = #{assetId}")
    Asset getAssetById(@Param("assetId")Integer assetId);
}
