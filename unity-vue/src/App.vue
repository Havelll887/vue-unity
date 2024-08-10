<template>
    <div id="app">
        <!-- <img alt="Vue logo" src="./assets/logo.png" /> -->
         <div class="box">
            <div class="btns" @click="sendMsg">点我给unity WEBGL 发消息</div>
            <div class="btns1">点击unity Cube 接收信息：{{ gettedMsg || '' }}</div>
         </div>

        <!-- <HelloWorld msg="Welcome to Your Vue.js App" /> -->
        <div class="temp">
            <iframe src="/dist/index.html" ref="unityvue"> </iframe>
        </div>
    </div>
</template>

<script>
// import HelloWorld from "./components/HelloWorld.vue";

export default {
    name: "App",
    mounted() { 
    },
    data(){
        return{
            gettedMsg:'还没有'
        }
    },
    methods: {
        refreshAllItem() {
            console.log('当前unity对象', this.$refs.unityvue.contentWindow.myGameInstance)
            // "unity场景内挂载脚本的物体名字" ， “方法名” ， “传递的参数”
            this.$refs.unityvue.contentWindow.myGameInstance.SendMessage('Cube', 'GetVueData', "256")
        },
        sendMsg(){
            this.refreshAllItem()
        }
    }
};
</script>

<style>
#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    margin-top: 60px;
    position: relative;
}

.temp {
    position: absolute;
    width: 100vw;
    height: 80vh;
}

iframe {
    width: 100%;
    height: 100%;
}


.box {
    display: flex;
}
.btns{
    padding: 5px 10px;
    margin: 10px;
    background-color: #b3b3b3;
    cursor: pointer;
}

.btns1{
    padding: 5px 10px;
    margin: 10px;
}
</style>
