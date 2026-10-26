# Lego Model Rendering

Model images such as `code/galdevweb/GaldevWeb/wwwroot/blog/de_lego-MarcoPolo.jpg` are rendered
from the WebGL viewer of the build instructions in headless Chrome.
The viewer lives in the separate repo `github-MarcoPoloInstructions` (locally `C:\Heiner\github-MarcoPoloInstructions`), export folder `dist/<model>/`.
The stand (chapter `stand`, 105 parts) is left out of blog images.

## Alternatives and their defects

- `thumbnail.png` inside the `.io` file (a zip) is a correct Stud.io render but always includes the stand.
- A Collada export from Stud.io is already Y-up.
  Applying the LDraw correction (negate Y) to it once more mirrors the model and shows it from below;
  the first blog image of October 2026 had exactly this defect (mirrored LEGO logos).

## 1. Copy the viewer export

Work on a copy, never inside the instructions repo:

```
mkdir render
cp C:\Heiner\github-MarcoPoloInstructions\dist\MarcoPolo2-27\{data.js,app.js,style.css} render\
```

## 2. Patch `render/app.js`

Two edits:

- In `renderToFBO`, make the background configurable:
  `gl.clearBufferfv(gl.COLOR, 0, [1, 1, 1, 1]);` → `gl.clearBufferfv(gl.COLOR, 0, window.SHOT_BG || [1, 1, 1, 1]);`
- At the end, replace `requestAnimationFrame(function () { buildPage(); });` with the hook below.
  It must stay inside the enclosing function, before the closing `})();`.

```js
window.renderShot = function (o) {
  var fin = D.chapters[D.chapters.length - 1];          /* complete model */
  var lo = -1, hi = -1, prev = 0;
  (fin.steps || []).forEach(function (st) {              /* step of chapter dropChapter (0 = stand) */
    if (st.sub && st.sub.chapter === (o.dropChapter == null ? -2 : o.dropChapter)) { lo = prev; hi = st.end; }
    prev = st.end;
  });
  var insts = fin.instances.filter(function (it, i) { return !(i >= lo && i < hi); });
  var sc = buildScene(insts, null);
  var W = o.w, H = o.h, S = o.ss || 2;
  var fbo = makeFBO(W * S, H * S); fbo.ss = S;
  var cam = makeCamera(sc, sc.total, o.az, o.el, null, o.margin == null ? 0.03 : o.margin);
  cam.zoom = o.zoom || 1; cam.panX = o.panX || 0; cam.panY = o.panY || 0;
  renderToFBO(fbo, sc, cam, null, [0, 0, W * S, H * S], Infinity, Infinity, false);
  var buf = new Uint8Array(W * S * H * S * 4);
  gl.bindFramebuffer(gl.FRAMEBUFFER, fbo.fb);
  gl.readBuffer(gl.COLOR_ATTACHMENT0);
  gl.readPixels(0, 0, W * S, H * S, gl.RGBA, gl.UNSIGNED_BYTE, buf);
  gl.bindFramebuffer(gl.FRAMEBUFFER, null);
  var c2 = document.createElement('canvas'); c2.width = W; c2.height = H;
  var ctx = c2.getContext('2d'), img = ctx.createImageData(W, H), dst = img.data, sw = W * S;
  for (var oy = 0; oy < H; oy++) for (var ox = 0; ox < W; ox++) {
    var r = 0, g = 0, b = 0;
    for (var sy = 0; sy < S; sy++) for (var sx = 0; sx < S; sx++) {
      var a = (((H - 1 - oy) * S + sy) * sw + ox * S + sx) * 4;   /* GL row 0 is the bottom */
      r += buf[a]; g += buf[a + 1]; b += buf[a + 2];
    }
    var k = S * S, q = (oy * W + ox) * 4;
    dst[q] = r / k; dst[q + 1] = g / k; dst[q + 2] = b / k; dst[q + 3] = 255;
  }
  ctx.putImageData(img, 0, 0);
  return { dropped: hi - lo, total: insts.length, canvas: c2 };
};
```

## 3. Create `render/render.html`

Every element id that `app.js` touches at startup must exist:

```html
<!DOCTYPE html>
<html><head><meta charset="utf-8"><title>shot</title>
<style>html,body{margin:0;padding:0;background:#000;overflow:hidden}#gl,#ui,#splash{display:none}#out{display:block}</style>
</head><body>
<canvas id="gl"></canvas>
<div id="ui"><div id="header"></div><div id="navbar"><button id="btn-home"></button><button id="btn-prev"></button><button id="btn-next"></button><select id="jump"></select><span id="nav-hint"></span><span id="nav-page"></span><button id="btn-reset"></button></div><div id="content"></div></div>
<div id="splash"></div>
<script src="data.js"></script>
<script>window.SHOT_BG = [0, 0, 0, 1];</script>
<script src="app.js"></script>
<script>
  var q = {}; location.search.slice(1).split('&').forEach(function (kv) { var p = kv.split('='); if (p[0]) q[p[0]] = decodeURIComponent(p[1] || ''); });
  var o = { w: +(q.w || 800), h: +(q.h || 600), az: +(q.az || 35), el: +(q.el || 30), ss: +(q.ss || 2),
            margin: q.margin != null ? +q.margin : 0.03, zoom: +(q.zoom || 1), panX: +(q.panX || 0), panY: +(q.panY || 0),
            dropChapter: q.keepstand ? null : 0 };
  var r = window.renderShot(o);
  r.canvas.id = 'out';
  document.body.appendChild(r.canvas);
  document.title = 'shot dropped=' + r.dropped + ' total=' + r.total;
</script>
</body></html>
```

## 4. Render with headless Chrome

```
"C:\Program Files\Google\Chrome\Application\chrome.exe" --headless=new --no-first-run --user-data-dir=%TEMP%\chrome-render --use-angle=swiftshader --enable-unsafe-swiftshader --ignore-gpu-blocklist --hide-scrollbars --window-size=1600,1200 --screenshot=final.png "file:///C:/path/to/render/render.html?w=1600&h=1200&az=60&el=30&ss=2&margin=0.03"
```

| Parameter | Meaning |
|-----------|---------|
| `--window-size` | equals `w` x `h`, the output size in pixels |
| `az`, `el` | azimuth and elevation in degrees; `az=60&el=30` matches the Stud.io thumbnail (antenna left, nozzle top right, big tanks front right) |
| `ss` | supersampling factor; `2` for the final image, `1` for quick previews |
| `margin` | border fraction around the model |
| `keepstand=1` | keeps the stand |

Preview small first (`w=800&h=600&ss=1`); one image takes seconds.

## 5. Convert to JPEG

```
python -c "from PIL import Image; Image.open('final.png').convert('RGB').save('de_lego-MarcoPolo.jpg', quality=90, optimize=True)"
```
