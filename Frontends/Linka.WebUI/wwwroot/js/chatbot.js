/* ============================================================
   Linka Assistant — front-end logic (vanilla JS, no dependencies)
   Talks to:  GET /Chatbot/Ask?message=...
   ============================================================ */
(function () {
    "use strict";

    var root = document.getElementById("linka-chat");
    if (!root) return;

    var toggle = document.getElementById("linka-chat-toggle");
    var minBtn = document.getElementById("linka-chat-min");
    var body = document.getElementById("linka-chat-body");
    var input = document.getElementById("linka-chat-text");
    var sendBtn = document.getElementById("linka-chat-send");

    var greeted = false;

    // ---- open / close ----
    function openChat() {
        root.classList.add("is-open");
        if (!greeted) { greeted = true; ask(""); } // empty message -> greeting
        setTimeout(function () { input.focus(); }, 200);
    }
    function closeChat() { root.classList.remove("is-open"); }
    function toggleChat() { root.classList.contains("is-open") ? closeChat() : openChat(); }

    toggle.addEventListener("click", toggleChat);
    minBtn.addEventListener("click", closeChat);

    // ---- send handlers ----
    function submit() {
        var text = input.value.trim();
        if (!text) return;
        addUserMessage(text);
        input.value = "";
        ask(text);
    }
    sendBtn.addEventListener("click", submit);
    input.addEventListener("keydown", function (e) {
        if (e.key === "Enter") { e.preventDefault(); submit(); }
    });

    // ---- talk to the server ----
    function ask(message) {
        var typing = addTyping();
        fetch("/Chatbot/Ask?message=" + encodeURIComponent(message), {
            headers: { "Accept": "application/json" }
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                typing.remove();
                renderBotReply(data);
            })
            .catch(function () {
                typing.remove();
                addBotMessage("Sorry, something went wrong. Please try again.");
            });
    }

    // ---- rendering ----
    function renderBotReply(data) {
        if (data.reply) addBotMessage(data.reply);

        if (data.products && data.products.length) {
            var wrap = el("div", "linka-products");
            data.products.forEach(function (p) { wrap.appendChild(productCard(p)); });
            body.appendChild(wrap);
        }

        if (data.navigateUrl) {
            var go = el("div", "linka-links");
            var a = el("a", "linka-link linka-link-go");
            a.href = data.navigateUrl;
            a.textContent = "Open this product →";
            go.appendChild(a);
            body.appendChild(go);

            // To auto-redirect on a single match instead of showing a button,
            // uncomment the next line:
            // window.location.href = data.navigateUrl;
        }

        if (data.links && data.links.length) {
            var lw = el("div", "linka-links");
            data.links.forEach(function (lnk) {
                var a = el("a", "linka-link");
                a.href = lnk.url;
                a.textContent = lnk.label;
                lw.appendChild(a);
            });
            body.appendChild(lw);
        }

        if (data.chips && data.chips.length) {
            var cw = el("div", "linka-chips");
            data.chips.forEach(function (c) {
                var b = el("button", "linka-chip");
                b.type = "button";
                b.textContent = c.label;
                b.addEventListener("click", function () {
                    addUserMessage(c.label);
                    ask(c.query);
                });
                cw.appendChild(b);
            });
            body.appendChild(cw);
        }

        scrollDown();
    }

    function productCard(p) {
        var a = el("a", "linka-product");
        a.href = p.url;

        var img = el("img", "linka-product-thumb");
        img.src = p.imageUrl || "";
        img.alt = p.name || "";
        img.loading = "lazy";
        img.onerror = function () { this.style.visibility = "hidden"; };
        a.appendChild(img);

        var info = el("div", "linka-product-info");

        var name = el("div", "linka-product-name");
        name.textContent = p.name || "";
        info.appendChild(name);

        if (p.category) {
            var cat = el("div", "linka-product-cat");
            cat.textContent = p.category;
            info.appendChild(cat);
        }

        var row = el("div", "linka-product-price-row");
        var price = el("span", "linka-product-price");
        price.textContent = p.price;
        row.appendChild(price);

        if (p.hasDiscount) {
            if (p.oldPrice) {
                var old = el("span", "linka-product-old");
                old.textContent = p.oldPrice;
                row.appendChild(old);
            }
            var badge = el("span", "linka-badge-disc");
            badge.textContent = "-%" + p.discountRate;
            row.appendChild(badge);
        }
        if (!p.inStock) {
            var out = el("span", "linka-badge-out");
            out.textContent = "Out of stock";
            row.appendChild(out);
        }
        info.appendChild(row);

        a.appendChild(info);
        return a;
    }

    // ---- message helpers ----
    function addUserMessage(text) {
        var m = el("div", "linka-msg linka-msg-user");
        m.textContent = text;
        body.appendChild(m);
        scrollDown();
    }
    function addBotMessage(text) {
        var m = el("div", "linka-msg linka-msg-bot");
        m.textContent = text;
        body.appendChild(m);
        scrollDown();
    }
    function addTyping() {
        var t = el("div", "linka-msg linka-msg-bot linka-typing");
        t.innerHTML = "<span></span><span></span><span></span>";
        body.appendChild(t);
        scrollDown();
        return t;
    }

    function el(tag, cls) {
        var e = document.createElement(tag);
        if (cls) e.className = cls;
        return e;
    }
    function scrollDown() { body.scrollTop = body.scrollHeight; }
})();
