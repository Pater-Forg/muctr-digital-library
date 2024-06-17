var url = "/Reader/GetFile?id=" + document.getElementById("file-path").value;

// Loaded via <script> tag, create shortcut to access PDF.js exports.
var { pdfjsLib } = globalThis;

// The workerSrc property shall be specified.
pdfjsLib.GlobalWorkerOptions.workerSrc = '/lib/pdf-js/pdf.worker.mjs';

var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 1.5,
    canvas = document.getElementById('pdf-render'),
    ctx = canvas.getContext('2d'),
    pageInput = document.getElementById('page-input');

/**
 * Get page info from document, resize canvas accordingly, and render page.
 * @param num Page number.
 */
function renderPage(num) {
    pageRendering = true;
    // Using promise to fetch the page
    pdfDoc.getPage(num).then(function (page) {
        var viewport = page.getViewport({ scale: scale });
        canvas.height = viewport.height;
        canvas.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRendering = false;
            if (pageNumPending !== null) {
                // New page rendering is pending
                renderPage(pageNumPending);
                pageNumPending = null;
            }
        });
    });

    // Update page counters
    document.getElementById('page-num').textContent = num;
}

/**
 * If another page rendering in progress, waits until the rendering is
 * finised. Otherwise, executes rendering immediately.
 */
function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}

/**
 * Displays previous page.
 */
function onPrevPage() {
    $("html, body").animate({ scrollTop: 0 }, "fast");
    if (pageNum <= 1) {
        return;
    }
    pageNum--;
    queueRenderPage(pageNum);
}
document.getElementById('prev-page').addEventListener('click', onPrevPage);

/**
 * Displays next page.
 */
function onNextPage() {
    $("html, body").animate({ scrollTop: 0 }, "fast");
    if (pageNum >= pdfDoc.numPages) {
        return;
    }
    pageNum++;
    queueRenderPage(pageNum);
}
document.getElementById('next-page').addEventListener('click', onNextPage);

/**
 * Asynchronously downloads PDF.
 */
pdfjsLib.getDocument(url).promise.then(function (pdfDoc_) {
    pdfDoc = pdfDoc_;
    document.getElementById('page-count').textContent = pdfDoc.numPages;
    pageNum = Number(document.getElementById('init-page').value);
    if (pageNum < 1) pageNum = 1;
    if (pageNum > pdfDoc.numPages) pageNum = pdfDoc.numPages;

    // Initial/first page rendering
    renderPage(pageNum);
});

function goToPage() {
    pageNum = Number(pageInput.value);
    if (pageNum >= pdfDoc.numPages) {
        pageNum = pdfDoc.numPages;
    }
    if (pageNum <= 1) {
        pageNum = 1;
    }
    queueRenderPage(pageNum);
    pageInput.value = pageNum;
}
document.getElementById('go-to-page').addEventListener('click', goToPage);