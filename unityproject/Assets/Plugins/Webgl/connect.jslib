mergeInto(LibraryManager.library,{   
    SelectedTarget: function (targetName) { 
        strs = UTF8ToString(targetName);
        UnityItemClick(strs);
    },

    TokenInvalid: function (str){
        str =  UTF8ToString(str);
        TokenInvalidFromUnity(str);
    }
});