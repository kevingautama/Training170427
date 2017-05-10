app.service("RestaurantService", function ($http) {
    this.getOrder = function () {
        return $http.get("/api/WaiterAPI")
    };
      
});

