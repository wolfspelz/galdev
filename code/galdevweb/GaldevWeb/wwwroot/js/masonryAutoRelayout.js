// Triggers Masonry re-layouts for each element having a data-masonry when
// contained image elements complete loading and their dimensions are known.

(() => {
    const enableLog = true;
    const detectionTimeoutMillisecs = 10e3;
    let firstDetectionAttemptTime = Date.now();
    const onLoadedRedrawMasonry = () => {
        if (typeof Masonry === 'undefined') {
            if (firstDetectionAttemptTime > Date.now() - detectionTimeoutMillisecs) {
                enableLog && console.log(`onLoadedRedrawMasonry: Masonry not detected (yet?) - resheduling detection...`);
                window.setTimeout(onLoadedRedrawMasonry, 100);
            } else {
                enableLog && console.log(`onLoadedRedrawMasonry: Masonry not detected - given up.`);
            }
        } else {
            enableLog && console.log(`onLoadedRedrawMasonry: Masonry detected.`);
            [...document.querySelectorAll('*[data-masonry]')].forEach(containerElem => {
                const imgElemsBox = [[...containerElem.querySelectorAll('img')]];
                enableLog && console.log(
                    `onLoadedRedrawMasonry: Found ${imgElemsBox[0].length} img elements found for Masonry-controlled container.`,
                    {containerElem, imgElems: imgElemsBox[0]});
                const refresher = () => {
                    enableLog && console.log(
                        `onLoadedRedrawMasonry: Checking ${imgElemsBox[0].length} images.`,
                        {containerElem, imgElems: imgElemsBox[0]});
                    const imgElemsLenOld = imgElemsBox[0].length;

                    imgElemsBox[0] = imgElemsBox[0].filter(imgElem => {
                        const dims = imgElem.getBoundingClientRect();
                        if (dims.left !== 0 || dims.right !== 0 || dims.top !== 0 || dims.bottom !== 0) {
                            enableLog && console.log(
                                `onLoadedRedrawMasonry: Image still loading.`, {containerElem, imgElem, dims});
                            return !imgElem.complete || dims.height === 0; // true if still loading.
                        } else {
                            enableLog && console.log(
                                `onLoadedRedrawMasonry: Image has incomplete dimensions.`,
                                {containerElem, imgElem, dims});
                            return false; // Dummy image without position or dimensions.
                        }
                    });

                    if (imgElemsBox[0].length !== imgElemsLenOld) {
                        enableLog && console.log(
                            `onLoadedRedrawMasonry: Some images done - triggering Masonry-relayout...`,
                            {containerElem});
                        window.setTimeout(() => new Masonry(containerElem), 100);
                    }
                    if (imgElemsBox[0].length === 0) {
                        enableLog && console.log(
                            `onLoadedRedrawMasonry: All images done for this container.`, {containerElem});
                    } else {
                        window.setTimeout(refresher, 100);
                    }
                };
                refresher();
            });
        }
    };
    if (document.readyState !== "loading") {
        onLoadedRedrawMasonry();
    } else {
        document.addEventListener("DOMContentLoaded", onLoadedRedrawMasonry);
    }
})();
