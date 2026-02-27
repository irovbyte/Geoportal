(function () {
    // ===== Theme persistence =====
    const keyTheme = "gp_theme";
    const html = document.documentElement;

    function applyTheme(t) { html.setAttribute("data-theme", t); }

    const savedTheme = localStorage.getItem(keyTheme);
    applyTheme(savedTheme || "system");

    document.querySelectorAll("[data-theme]").forEach(btn => {
        btn.addEventListener("click", () => {
            const t = btn.getAttribute("data-theme");
            localStorage.setItem(keyTheme, t);
            applyTheme(t);
        });
    });

    // ===== Region persistence =====
    const keyRegion = "gp_region";
    const regionBtn = document.getElementById("gpRegionBtn");

    function setRegion(name) {
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

    // ===== Header height: robust (no jumps) =====
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

    // ===== Utils =====
    function textNorm(s) {
        return (s || "").replace(/\s+/g, " ").trim().toLowerCase();
    }

    function findCardByTitleContains(needle) {
        const n = textNorm(needle);
        const candidates = Array.from(document.querySelectorAll("h1,h2,h3,h4,.card-title,.gp-card-h"));
        const hit = candidates.find(el => textNorm(el.textContent).includes(n));
        if (!hit) return null;
        return hit.closest(".gp-card, .card");
    }

    // ===== Apply gradients exactly like mock =====
    // 1) Big top summary: same as header, more saturated
    const mon = findCardByTitleContains("Мониторинг инфраструктуры");
    if (mon) {
        mon.classList.add("gp-grad-h", "gp-grad-strong");
    }

    // 2) Map preview: diagonal collage
    const mapPrev = findCardByTitleContains("Карта (превью)");
    if (mapPrev) {
        mapPrev.classList.add("gp-grad-d");
    }

    // 3) Latest requests: MUST remain adaptive plain (white/dark)
    const last = findCardByTitleContains("Последние обращения");
    if (last) {
        last.classList.remove("gp-grad-h", "gp-grad-d", "gp-grad-strong");
    }

    // ===== Auto color +12% / -12% =====
    function colorizeDeltas(scope) {
        const root = scope || document;
        const nodes = root.querySelectorAll("*");
        nodes.forEach(el => {
            if (!el || !el.childNodes || el.childNodes.length !== 1) return;
            if (el.children && el.children.length) return;

            const txt = (el.textContent || "").trim();
            const m = txt.match(/^([+\-])\s*\d+(\.\d+)?\s*%$/);
            if (!m) return;

            el.classList.add("gp-delta");
            el.classList.remove("pos", "neg");
            if (m[1] === "+") el.classList.add("pos");
            if (m[1] === "-") el.classList.add("neg");
        });
    }
    colorizeDeltas();

    // ===== Make KPI mini cards white automatically (inside Monitoring block) =====
    if (mon) {
        const innerCards = mon.querySelectorAll(".gp-card");
        innerCards.forEach(c => {
            if (c === mon) return;
            // only small inner boxes
            c.classList.add("gp-kpi");
        });
    }

    // ===== Keep fixes on dynamic changes =====
    if (window.MutationObserver) {
        const mo = new MutationObserver(() => {
            const mon2 = findCardByTitleContains("Мониторинг инфраструктуры");
            if (mon2) mon2.classList.add("gp-grad-h", "gp-grad-strong");

            const mapPrev2 = findCardByTitleContains("Карта (превью)");
            if (mapPrev2) mapPrev2.classList.add("gp-grad-d");

            const last2 = findCardByTitleContains("Последние обращения");
            if (last2) last2.classList.remove("gp-grad-h", "gp-grad-d", "gp-grad-strong");

            colorizeDeltas();

            if (mon2) {
                mon2.querySelectorAll(".gp-card").forEach(c => {
                    if (c !== mon2) c.classList.add("gp-kpi");
                });
            }
        });
        mo.observe(document.body, { childList: true, subtree: true });
    }
})();