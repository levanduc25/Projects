// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// @ts-nocheck

document.addEventListener("DOMContentLoaded", function () {
  const toggle = document.getElementById("sidebarToggle");
  if (toggle) {
    toggle.addEventListener("click", function () {
      document.getElementById("sidebar").classList.toggle("d-none");
    });
  }
});

document.addEventListener("DOMContentLoaded", function () {
  const chartElement = document.getElementById("saleChart");
  const noDataMsg = document.getElementById("noDataMessage");
  if (chartElement) {
    fetch("/Admin/Home/GetSalesChart")
      .then((response) => response.json())
      .then((data) => {
        if (!data || data.length === 0) {
          chartElement.style.display = "none";
          if (noDataMsg) noDataMsg.style.display = "block";
          return;
        }

        const labels = data.map((d) => d.date);
        const values = data.map((d) => d.totalSales);

        const ctx = chartElement.getContext("2d");
        new Chart(ctx, {
          type: "line",
          data: {
            labels: labels,
            datasets: [
              {
                label: "Sale Details",
                data: values,
                borderColor: "rgba(54, 162, 235, 1)",
                backgroundColor: "rgba(54, 162, 235, 0.2)",
                tension: 0.3,
                fill: true,
              },
            ],
          },
          options: {
            responsive: true,
            scales: { y: { beginAtZero: true } },
          },
        });
      })
      .catch((err) => {
        console.error("Error:", err);
        chartElement.style.display = "none";
        if (noDataMsg) {
          noDataMsg.innerText = "No data";
          noDataMsg.style.display = "block";
        }
      });
  }
});

function addLabel() {
    let name = prompt("Nhập tên nhãn mới:");
    if (name && name.trim() !== "") {
        // chọn màu
        let colorInput = document.createElement("input");
        colorInput.type = "color";
        colorInput.value = "#999999"; // màu mặc định
        colorInput.style.position = "absolute";
        colorInput.style.left = "-9999px"; // ẩn input

        document.body.appendChild(colorInput);

        // mở color picker
        colorInput.click();

        colorInput.addEventListener("input", function() {
            let chosenColor = this.value;

            // Tạo element li
            let li = document.createElement("li");
            li.className = "mb-2 d-flex align-items-center label-item";
            li.setAttribute("onclick", "this.classList.toggle('active')");

            // span ô vuông
            let box = document.createElement("span");
            box.className = "label-box";
            box.style.borderColor = chosenColor;

            // span text
            let text = document.createElement("span");
            text.textContent = name;

            li.appendChild(box);
            li.appendChild(text);

            document.getElementById("labelList").appendChild(li);

            document.body.removeChild(colorInput); // xóa input sau khi dùng
        });
    }
}
