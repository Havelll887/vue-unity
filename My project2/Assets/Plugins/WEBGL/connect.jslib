mergeInto(LibraryManager.library,{   
    PostScore: function (sceneName) { 
     strs = UTF8ToString(sceneName);
     GetScore(strs);                       
    },    
});