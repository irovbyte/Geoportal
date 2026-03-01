(function () {

  const keyTheme = "gp_theme";
  const html = document.documentElement;

  function applyTheme(t) {
    html.setAttribute("data-theme", t);
  }

  const savedTheme = localStorage.getItem(keyTheme);
  applyTheme(savedTheme || "system");

  document.querySelectorAll("[data-theme]").forEach(btn => {
    btn.addEventListener("click", () => {
      const t = btn.getAttribute("data-theme");
      localStorage.setItem(keyTheme, t);
      applyTheme(t);
    });
  });

  const keyRegion = "gp_region";
  const regionBtn = document.getElementById("gpRegionBtn");

  function setRegion(name){
    if (!regionBtn) return;
    regionBtn.textContent = "Регион: " + name;
  }

  const savedRegion = localStorage.getItem(keyRegion);
  if (savedRegion) setRegion(savedRegion);

  document.querySelectorAll("[data-region]").forEach(item => {
    item.addEventListener("click", () => {
      const r = item.getAttribute("data-region");
      if (!r) return;
      localStorage.setItem(keyRegion, r);
      setRegion(r);
    });
  });

  const header = document.getElementById("gpHeader");

  function setHeaderHeight() {
    if (!header) return;
    const h = Math.ceil(header.getBoundingClientRect().height);
    document.documentElement.style.setProperty("--gp-header-h", h + "px");
  }

  setHeaderHeight();
  requestAnimationFrame(setHeaderHeight);

  window.addEventListener("load", () => {
    setHeaderHeight();
    requestAnimationFrame(setHeaderHeight);
  });

  if (header && window.ResizeObserver) {
    const ro = new ResizeObserver(() => setHeaderHeight());
    ro.observe(header);
  } else {
    window.addEventListener("resize", setHeaderHeight);
  }

})();