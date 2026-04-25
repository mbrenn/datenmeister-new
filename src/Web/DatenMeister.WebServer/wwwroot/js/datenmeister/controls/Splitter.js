export class Splitter {
    /**
     * Initializes the splitter on a given container
     * @param containerSelector The selector for the container element
     */
    static init(containerSelector) {
        const container = document.querySelector(containerSelector);
        if (!container)
            return;
        const leftSide = container.querySelector('.dm-splitter-left');
        const resizer = container.querySelector('.dm-splitter-resizer');
        if (!leftSide || !resizer)
            return;
        let x = 0;
        let leftWidth = 0;
        const mouseDownHandler = (e) => {
            x = e.clientX;
            leftWidth = leftSide.getBoundingClientRect().width;
            document.addEventListener('mousemove', mouseMoveHandler);
            document.addEventListener('mouseup', mouseUpHandler);
            resizer.classList.add('resizing');
            document.body.style.cursor = 'col-resize';
            // Disable text selection during resize
            document.body.style.userSelect = 'none';
        };
        const mouseMoveHandler = (e) => {
            const dx = e.clientX - x;
            const newLeftWidth = leftWidth + dx;
            leftSide.style.width = `${newLeftWidth}px`;
        };
        const mouseUpHandler = () => {
            resizer.classList.remove('resizing');
            document.body.style.removeProperty('cursor');
            document.body.style.removeProperty('user-select');
            document.removeEventListener('mousemove', mouseMoveHandler);
            document.removeEventListener('mouseup', mouseUpHandler);
        };
        resizer.addEventListener('mousedown', mouseDownHandler);
    }
}
//# sourceMappingURL=Splitter.js.map