var WebGLLocalStorage = {
    SaveToLocalStorage: function(key, data) {
        localStorage.setItem(Pointer_stringify(key), Pointer_stringify(data));
    },

    LoadFromLocalStorage: function(key) {
        var item = localStorage.getItem(Pointer_stringify(key));
    
		if (item === null) {
			// If the item is not found in local storage, return an empty string.
			return allocate(intArrayFromString("empty"), 'i8', ALLOC_STACK);
		}
    
		return allocate(intArrayFromString(item), 'i8', ALLOC_STACK);
    },

    RemoveFromLocalStorage: function(key) {
        localStorage.removeItem(Pointer_stringify(key));
    },

    ClearLocalStorage: function() {
        localStorage.clear();
    }
};
mergeInto(LibraryManager.library, WebGLLocalStorage);