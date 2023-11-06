package com.example.dao;

import com.example.entity.Asset;
import com.example.entity.Params;
import com.example.entity.User;
import org.apache.ibatis.annotations.Param;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.Update;
import org.springframework.stereotype.Repository;
import tk.mybatis.mapper.common.Mapper;

import java.util.List;

@Repository
public interface AssetDao extends Mapper<Asset> {

    // 1. 基于注解的方式
    //@Select("select * from user")

    List<Asset> findBySearch(@Param("params")Params params);

    @Select("delete from assets where assetId = #{assetId} limit 1")
    void deleteByAssetId(@Param("assetId") Integer assetId);

//    @Update("update assets set assetName = #{assetNewName} where assetId = #{assetId}")
//    void updateAssetNameById(@Param("assetId") Integer assetId, @Param("assetNewName") String assetNewName);
    @Update("update assets set assetName = #{assetNewName,jdbcType=VARCHAR} where assetId = #{assetId,jdbcType=INTEGER}")
    void updateAssetNameById(@Param("assetId") Integer assetId, @Param("assetNewName") String assetNewName);

    default void addAsset(Asset asset){}
}
