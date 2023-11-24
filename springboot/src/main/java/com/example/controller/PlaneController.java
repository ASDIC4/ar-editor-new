// 编写 by 赵嘉诚
package com.example.controller;

import com.example.common.Result;
import com.example.entity.PlaneObject;
import com.example.service.AssetService;
import com.example.service.PlaneObjectService;
import org.springframework.web.bind.annotation.*;

import javax.annotation.Resource;

@CrossOrigin
@RestController
@RequestMapping("/plane")
public class PlaneController {

    @Resource
    private PlaneObjectService planeObjectService;

    @PostMapping("/upload/{assetId}/{objectId}/{positionX}/{positionY}/{positionZ}/{rotationX}/{rotationY}/{rotationZ}/{scaleX}/{scaleY}/{scaleZ}/")
    public Result insertPlaneObject(@PathVariable Integer assetId, @PathVariable Integer objectId,
                                    @PathVariable Float positionX, @PathVariable Float positionY, @PathVariable Float positionZ,
                                    @PathVariable Float rotationX, @PathVariable Float rotationY, @PathVariable Float rotationZ,
                                    @PathVariable Float scaleX, @PathVariable Float scaleY, @PathVariable Float scaleZ) {
        PlaneObject newPlaneObject  = new PlaneObject(assetId, objectId, positionX, positionY, positionZ,
                rotationX, rotationY, rotationZ, scaleX, scaleY, scaleZ);

        planeObjectService.insertPlaneObject(newPlaneObject);
        return Result.success();
    }
}
