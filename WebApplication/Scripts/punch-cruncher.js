angular.module('punchCruncher', ['ui.bootstrap'])
    .controller('punchCruncherController', function ($scope, $http) {
        $scope.request = {
            'PunchIn': new Date(),
            'LunchOut': new Date(),
            'LunchIn': new Date(),
            'TargetTotalMinutes': 480
        };

        $scope.request.PunchIn.setHours(8, 0, 0);
        $scope.request.LunchOut.setHours(12, 0, 0);
        $scope.request.LunchIn.setHours(12, 30, 0);

        $scope.calculate = function (request) {
            $scope.success = $scope.error = null;

            $http.post('/api/calculation', request).success(function (data) {
                if (data.IsSuccessful) {
                    var resultDate = new Date(data.Result);
                    $scope.success = resultDate.toLocaleTimeString();
                } else {
                    $scope.error = data.Result;
                }
            });
        };
    });