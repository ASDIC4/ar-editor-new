<!-- writtern by 赵嘉诚 -->
<template>
  <div class="asset">
    <div>
      <template>
        <el-select
          v-model="params.assetType"
          filterable
          placeholder="请选择数据类型"
          style="width: 150px; margin-right: 10px"
          @change="handleAssetTypesChange"
        >
          <el-option
            v-for="item in assetTypes"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          >
          </el-option>
        </el-select>
      </template>
      <el-input
        v-model="params.assetName"
        style="width: 150px; margin-right: 10px"
        placeholder="请输入素材名"
        @input="handleSearch()"
      ></el-input>
      <el-input
        v-model="params.assetId"
        style="width: 150px; margin-right: 10px"
        placeholder="请输入素材id"
        @input="handleSearch()"
      ></el-input>

      <el-button @click="reset()">刷新</el-button>
      <el-button type="primary" @click="uploadDialogVisible = true">
        上传</el-button
      >

      <el-dialog title="上传素材" :visible.sync="uploadDialogVisible">
        <el-form :model="form">
          <el-form-item label="素材名" :label-width="formLabelWidth">
            <el-input v-model="form.assetName" autocomplete="off" style="width: 210px"></el-input>
          </el-form-item>
          <el-form-item label="素材类型" :label-width="formLabelWidth">
            <el-select v-model="form.assetType" placeholder="请选择素材类型">
              <el-option label="模型" value="model"></el-option>
              <el-option label="图片" value="picture"></el-option>
              <el-option label="视频" value="video"></el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="上传文件" :label-width="formLabelWidth">
            <el-upload
              class="upload-demo"
              :action="getUploadUrl()"
              :on-success="successUpload"
            >
              <el-button type="primary" size="small">点击上传</el-button>
            </el-upload>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="cancelUpload()">取 消</el-button>
          <el-button type="primary" @click="confirmUpload()">确 定</el-button>
        </div>
      </el-dialog>

      <!-- <el-button type="primary">上传</el-button> -->
    </div>
    <el-table :data="tableData" style="width: 100%">
      <!-- <el-table-column prop="thumbPath" label="缩略图" width="180">
        <template slot-scope="scope">
           使用img标签展示照片 -->
      <!-- <img :src="scope.row.thumbPath" alt="照片" />
        </template>
      </el-table-column> -->
      
      <el-table-column prop="assetPath" label="缩略图" width="180">
        <!-- <el-image
          style="width: 70px; height: 70px; border-radius: 50%"
          :src= "scope.row.assetPath"
          :preview-src-list= "scope.row.assetPath">
        </el-image> -->
        <template slot-scope="scope">
            <el-image
            style="width: 70px; height: 70px; border-radius: 50%"
            :src="getThumbnailUrl(scope.row.assetPath)"
            :preview-src-list="[getThumbnailUrl(scope.row.assetPath)]"
            @error="handleImageError"
          ></el-image>
        </template>
      </el-table-column>
    
      <el-table-column prop="assetName" label="素材名" width="180">
      </el-table-column>
      <el-table-column prop="assetId" label="素材ID"> </el-table-column>
      <el-table-column prop="assetType" label="素材类型">
        <template slot-scope="scope">
          {{ mapAssetType(scope.row.assetType) }}
        </template>
      </el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
            <el-button type="primary" @click="showEditDialog(scope.row)">编辑</el-button>
            <el-button type="danger" @click="showDeleteDialog(scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog title="修改素材名" :visible.sync="editDialogVisible">
      <el-form >
        <el-form-item label="新名" :label-width="formLabelWidth">
          <el-input v-model="editAssetNewName" autowcomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="原名" :label-width="formLabelWidth">
          {{this.editTarget.assetName}}
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="edit">确认</el-button>
        <el-button @click="editDialogVisible = false">取消</el-button>
      </div>
    </el-dialog>

    <el-dialog
      title="素材管理"
      :visible.sync="deleteDialogVisible"
      width="30%"
      @close="handleDeleteDialogClose"
    >
      <span>确定要删除素材{{ deleteTarget.assetName }}吗？</span>
      <span slot="footer" class="dialog-footer">
        <el-button type="danger" @click="confirmDelete">确认</el-button>
        <el-button @click="deleteDialogVisible = false">取消</el-button>
      </span>
    </el-dialog>

    <div style="margin-top: 15px">
      <!--            <span class="demonstration">完整功能</span>-->
      <el-pagination
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
        :current-page="params.pageNum"
        :page-sizes="[5, 10, 15, 20]"
        :page-size="params.pageSize"
        layout="total, sizes, prev, pager, next"
        :total="total"
      >
      </el-pagination>
    </div>
  </div>
</template>


<script>
import request from "@/utils/request";

export default {
  data() {
    return {
      params: {
        assetName: "",
        assetId: "",
        userId: "",
        assetType: "",
        pageNum: 1,
        pageSize: 5,
      },
      // 这是搜索用的
      tableData: [],
      total: 100,
      uploadDialogVisible: false,
      form: {
        assetName: "",
        assetId: "",
        assetType: "",
        assetPath: "",
      },
      formLabelWidth: "100px",
      fileList: [
        {
          name: "food.jpeg",
          url: "https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100",
        },
      ],
      assetTypes: [
        { label: "模型", value: "model" },
        { label: "图片", value: "picture" },
        { label: "视频", value: "video" },
      ],
      editDialogVisible: false,
      deleteDialogVisible: false, // 控制删除确认对话框的显示状态
      deleteTarget: [], // 保存用户点击删除时的目标数据
      editTarget: [],
      editAssetNewName: "",
      nowFileStoragePath: "",
    };
  },
  created() {
    this.findBySearch();
  },
  methods: {
    // load() {
    //   request.get("/model").then((res) => {
    //     if (res.code === "0") {
    //       this.tableData = res.data.map((item) => {
    //         // 将相对路径拼接为完整的图片 URL //只有初始的时候写成这样，暂时
    //         item.thumbPath = process.env.BASE_URL + "/images/" + item.thumbPath;
    //         return item;
    //       });
    //     } else {
    //     }
    //   });
    // },
    findBySearch() {
      const user = sessionStorage.getItem("user");
      this.params.userId = JSON.parse(user).userId;
      console.log("发送的参数：", this.params);
      request
        .get("/asset/search", {
          params: this.params,
        })
        .then((res) => {
          if (res.code === "0") {
            this.tableData = res.data.list;
            this.total = res.data.total;
            // console.log("康康什么类型");
            console.log(res.data);
          } else {
          }
        });
    },
    handleSearch() {
      this.findBySearch();
    },
    handleAssetTypesChange(selectedType) {
      this.params.assetType = selectedType;
      console.log("查看selectedTypes参数:", selectedType);
      this.findBySearch();
    },
    reset() {
      this.params = {
        assetName: "",
        assetId: "",
        userId: "",
        assetType: "",
        pageNum: 1,
        pageSize: 5,
      };
      const user = sessionStorage.getItem("user");
      this.params.userId = JSON.parse(user).userId;
      this.findBySearch();
    },
    
    handleSizeChange(pageSize) {
      this.params.pageSize = pageSize;
      this.findBySearch();
    },
    handleCurrentChange(pageNum) {
      this.params.pageNum = pageNum;
      this.findBySearch();
    },
    // handleUploadSuccess(response, file, fileList) {
    //   // 从服务器返回的数据中获取上传文件的名称，假设服务器返回的文件名字段为 'fileName'
    //   this.uploadedFileName = response.fileName;
    // },
    // handleUploadSuccess(response, file, fileList) {
    //   // 上传成功后，将文件信息存储在 form.files 中
    //   // response 包含后台返回的上传成功信息
    //   // file 包含当前上传的文件信息
    //   this.form.files.push({
    //     name: file.name, // 文件名
    //     url: response.url, // 上传后的文件URL，需要根据后台返回的数据结构进行调整
    //     // 其他文件信息...
    //   });
    // },
    successUpload(res){
      console.log("看看四件戳res");
      this.nowFileStoragePath = res.data;
      console.log(this.nowFileStoragePath);
    },
    handleExceed(files, fileList) {
      this.$message.warning(
        `当前限制选择 3 个文件，本次选择了 ${files.length} 个文件，共选择了 ${
          files.length + fileList.length
        } 个文件`
      );
    },
    beforeRemove(file, fileList) {
      return this.$confirm(`确定移除 ${file.name}？`);
    },
    confirmUpload(){
      this.uploadDialogVisible = false;

      request.post("/asset/confirmUpload/"+this.form.assetName +"/"+this.form.assetType+"/" + this.params.userId+ "/"+ this.nowFileStoragePath).then((res) => {
        if (res.code === "0") {
          this.$message({
          message: "成功上传",
          type: "success",
          });
        } else {
          }
      });
      this.handleSearch();
    },
    cancelUpload(){
        this.uploadDialogVisible = false;
        request.delete("/asset/cancelUpload/"+this.nowFileStoragePath).then((res) => {
        if (res.code === "0") {
          this.$message({
            message: "成功取消上传",
            type: "success",
          });
        } else {
        }
      });
    },
    mapAssetType(type) {
      const typeMap = {
        picture: "图片",
        video: "视频",
        model: "模型",
        // 可以根据需要添加更多的映射关系
      };
      return typeMap[type] || type;
    },
    showEditDialog(row) {
      console.log("打开edit dialog")
      if (row) {
        this.editTarget = row; // 保存待删除的数据
      } else {
        this.editTarget = []
      }
      this.editDialogVisible = true; // 打开编辑确认对话框
    },
    edit() {
      console.log("我来看看")
      console.log(this.editAssetNewName);

      //传一个老的assetId，和新的assetName
      if (this.editTarget != [] && this.editAssetNewName) {
        console.log("editTatget有值")
        request.post("/asset/edit/" + this.editTarget.assetId+"/"+ this.editAssetNewName).then(res => {
          if (res.code === '0') {
            this.$message({
              message: '编辑成功',
              type: 'success'
            });
            this.findBySearch();
          } else {
            this.$message({
              message: res.msg,
              type: 'fail'
            });
          }
        })
      }
      this.editAssetNewName = "";
      this.editTarget = [];
      this.editDialogVisible = false;
      // console.log("哪里有问题")
      // this.findBySearch();
      // console.log("看一下为啥")
    },
    showDeleteDialog(row) {
      if(row){
        this.deleteTarget = row; // 保存待删除的数据
      }else {
        this.deleteTarget = []
      }
      
      this.deleteDialogVisible = true; // 打开删除确认对话框
    },
    confirmDelete() {
      // 在这里执行删除操作，使用 this.deleteTarget 获取待删除数据的信息
      console.log("删除数据:", this.deleteTarget);
      // 执行实际的删除操作，可以调用 API 或其他方法
      // ...
      if(this.deleteTarget != []){
        request.delete("/asset/delete/" + this.deleteTarget.assetId).then(res => {
          if (res.code === '0') {
            this.$message({
              message: '删除成功',
              type: 'success'
            });
            this.findBySearch();
          } else {
            this.$message({
              message: res.msg,
              type: 'fail'
            });
          }
        })
      }
      // 删除完成后关闭对话框
      this.deleteDialogVisible = false;
      // 清空 deleteTarget，以便下次使用
      this.deleteTarget = [];
      this.findBySearch()
    },
    handleDeleteDialogClose() {
      // 在对话框关闭时清空 deleteTarget
      this.deleteTarget = [];
    },
    getThumbnailUrl(assetPath) {
      return `http://47.93.242.88:8080/api/asset/${assetPath}`;
    },
    handleImageError(event) {
      // This method is called when the image fails to load
      // You can update the source to a fallback image here
      event.target.src = 'http://47.93.242.88:8080/api/asset/error.jpg';
    },
    getUploadUrl(){
      return `http://47.93.242.88:8080/api/asset/upload`
    }
  },
};
</script>

