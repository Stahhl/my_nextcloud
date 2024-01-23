/**
 *
 * (c) Copyright Ascensio System SIA 2020
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

// Example insert text into editors (different implementations)
(function (window, undefined) {
    window.Asc.plugin.init = async function () {
        console.log("init");

        try {
            this.callCommand(function () {
                var fullname = Api.GetFullName();
                console.log("fullname: " + fullname);

                console.log("futurama")
                fetch("https://api.sampleapis.com/futurama/info")
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("Network response was not ok");
                        }
                        console.log(response.json())
                    })
                    .catch(error => {
                        console.error("Error fetching data:", error);
                    });
            }, true);
        } catch (error) {
            console.error("Error:", error);
        }
    };

    window.Asc.plugin.button = function (id) {
        console.log("button");
    };
})(window, undefined);

