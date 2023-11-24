package com.example.entity;

import javax.persistence.Column;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

public class Mapp {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer mapId;

    @Column(name = "userId")
    private Integer userId;
    @Column(name = "easyarId")
    private String easyarId;
    @Column(name = "easyarName")
    private String easyarName;

    public Integer getMapId() {
        return mapId;
    }

    public void setMapId(Integer mapId) {
        this.mapId = mapId;
    }

    public Integer getUserId() {
        return userId;
    }

    public void setUserId(Integer userId) {
        this.userId = userId;
    }

    public String getEasyarId() {
        return easyarId;
    }

    public void setEasyarId(String easyarId) {
        this.easyarId = easyarId;
    }

    public String getEasyarName() {
        return easyarName;
    }

    public void setEasyarName(String easyarName) {
        this.easyarName = easyarName;
    }

    public Mapp() {
    }

    public Mapp(Integer mapId, Integer userId, String easyarId, String easyarName) {
        this.mapId = mapId;
        this.userId = userId;
        this.easyarId = easyarId;
        this.easyarName = easyarName;
    }
}