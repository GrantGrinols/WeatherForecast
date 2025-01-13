// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    
    function showContainer(containerId){
        
        const containers = document.querySelectorAll('.pagecontainer');
        console.log(containers.length);
        containers.forEach(container => {
            container.classList.remove('active');
        });

        document.getElementById(containerId).classList.add('active');
    }
