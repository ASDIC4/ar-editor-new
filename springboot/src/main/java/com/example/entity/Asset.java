package com.example.entity;

import javax.persistence.*;

@Table(name = "assets")
public class Asset {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer assetId;

    @Column(name = "userId")
    private Integer userId;
    @Column(name = "assetName")
    private String assetName;

    @Column(name = "assetPath")
    private String assetPath;
    @Column(name = "assetType")
    private String assetType;

    public Integer getAssetId() {
        return assetId;
    }

    public void setAssetId(Integer assetId) {
        this.assetId = assetId;
    }

    public Integer getUserId() {
        return userId;
    }

    public void setUserId(Integer userId) {
        this.userId = userId;
    }

    public String getAssetName() {
        return assetName;
    }

    public void setAssetName(String assetName) {
        this.assetName = assetName;
    }


    public void setAssetPath(String assetUrl) {
        this.assetPath = assetUrl;
    }

    public String getAssetType() {
        return assetType;
    }

    public void setAssetType(String assetType) {
        this.assetType = assetType;
    }

    public String getAssetPath() {
        return assetPath;
    }

    public Asset() {
    }

    public Asset(Integer assetId, Integer userId, String assetName, String assetPath, String assetType) {
        this.assetId = assetId;
        this.userId = userId;
        this.assetName = assetName;
        this.assetPath = assetPath;
        this.assetType = assetType;
    }
}