<template>
    <div class="model">
        <div>
            <el-input  style="width: 250px; margin-right: 15px"
                       placeholder="请输入内容"></el-input>
            <el-button >搜索</el-button>
            <el-button type="primary">新增</el-button>
        </div>
        <el-table
            :data="tableData"
            stripe
            style="width: 100%">
            <el-table-column
                prop="thumbnail"
                label="缩略图"
                width="180">
            </el-table-column>
            <el-table-column
                    prop="name"
                    label="素材名"
                    width="180">
            </el-table-column>
            <el-table-column
                    prop="assetsId"
                    label="素材ID">
            </el-table-column>
            <el-table-column label="操作">
                <el-button type="primary">编辑</el-button>
                <el-button type="danger">删除</el-button>
            </el-table-column>
        </el-table>
    </div>
</template>

<script>
  export default {
    data() {
      return {
        tableData: [{
          thumbnail: '2016-05-02',
          name: '模型1',
          assetsId: 'adkasdnininon221'
        }, {
          thumbnail: '2016-05-04',
          name: '模型2',
          assetsId: 'adkasdnininon221'
        }, {
          thumbnail: '2016-05-01',
          name: '模型3',
          assetsId: 'adkasdnininon221'
        }, {
          thumbnail: '2016-05-03',
          name: '模型4',
          assetsId: 'adkasdnininon221'
        }]
      }
    }
  }
</script>