<!-- writtern by 赵嘉诚 -->
<template>
    <div class="model">
        <el-table
            :data="tableData"
            stripe
            style="width: 100%">
            
            <el-table-column
                    prop="easyarName"
                    label="地图名"
                    width="180">
            </el-table-column>
            <el-table-column
                    prop="easyarId"
                    label="地图ID">
            </el-table-column>
            <el-table-column label="操作">
              <template slot-scope="scope">
               <el-button type="danger" @click="showDeleteDialog(scope.row)">删除</el-button>
              </template>
            </el-table-column>
        </el-table>

        <el-dialog
          title="素材管理"
          :visible.sync="deleteDialogVisible"
          width="30%"
          @close="handleDeleteDialogClose"
        >
          <span>确定要删除素材{{ deleteTarget.easyarId }}吗？</span>
          <span slot="footer" class="dialog-footer">
            <el-button type="danger" @click="confirmDelete">确认</el-button>
            <el-button @click="deleteDialogVisible = false">取消</el-button>
          </span>
        </el-dialog>
    </div>
</template>

<script>
import request from "@/utils/request";

  export default {
    data() {
      return {
        tableData: [],
        userId: "",
        deleteDialogVisible: false, // 控制删除确认对话框的显示状态
        deleteTarget: [], // 保存用户点击删除时的目标数据
      }
    },
    created() {
      this.findBySearch();
    },
    methods: {
      findBySearch() {
        const user = sessionStorage.getItem("user");
        this.userId = JSON.parse(user).userId;
        console.log("发送的scene参数：", this.userId);
        request
          .get("/scene/search/"+this.userId)
          .then((res) => {
            if (res.code === "0") {
              this.tableData = res.data;
              console.log("康康什么类型");
              console.log(res.data);
            } else {
            }
          });
      },
      reset() {
        const user = sessionStorage.getItem("user");
        this.params.userId = JSON.parse(user).userId;
        this.findBySearch();
      },
      showDeleteDialog(row) {
        if (row) {
          this.deleteTarget = row; // 保存待删除的数据
        } else {
          this.deleteTarget = []
        }
        this.deleteDialogVisible = true; // 打开删除确认对话框
      },
      confirmDelete() {
        // 在这里执行删除操作，使用 this.deleteTarget 获取待删除数据的信息
        console.log("删除数据:", this.deleteTarget);
        // 执行实际的删除操作，可以调用 API 或其他方法
        // ...
        if (this.deleteTarget != []) {
          request.delete("/scene/" + this.deleteTarget.mapId).then(res => {
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

    }
  }
</script>