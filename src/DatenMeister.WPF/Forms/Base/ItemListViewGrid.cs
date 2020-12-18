using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using DatenMeister.WPF.Forms.Base.GridControl;

namespace DatenMeister.WPF.Forms.Base
{
    namespace GridControl
    {
        /// <summary>
        /// Gets the column definition
        /// </summary>
        public class GridColumnDefinition
        {
            public int Width { get; set; }
            
            public string Title { get; set; } = string.Empty;
        }

        /// <summary>
        /// Defines the element for the cell
        /// </summary>
        public class CellInstantiation
        {
            public FrameworkElement? CellElement { get; set; }
        }

        public class RowInstantiation
        {
            public double Height { get; set; }

            public double PositionOffset { get; set; }
            
            public Brush? BackgroundColor { get; set; }

            public List<CellInstantiation> Cells { get; } = new List<CellInstantiation>();
        }

        public class ColumnInstantiation
        {
            public double PositionOffset { get; set; }
            public double Width { get; set; }

            public GridColumnDefinition? ColumnDefinition { get; set; }
        }
    }

    /// <summary>
    /// Create the grid view
    /// </summary>
    public class ItemListViewGrid : DockPanel
    {
        private readonly Canvas _canvas;
        private readonly ScrollBar _scrollVertical;
        private readonly ScrollBar _scrollHorizontal;
        private readonly DockPanel _contentPanel;

        public int RowHeight { get; set; } = GridSettings.RowHeight;

        private readonly List<RowInstantiation> _rowInstantiations = new List<RowInstantiation>();

        private readonly List<ColumnInstantiation> _columnInstantiations = new List<ColumnInstantiation>();
        private int _rowOffset;
        private int _columnOffset;

        public static class GridSettings
        {
            public const int GridPenWidth = 1;
            public static readonly Brush GridBrush = Brushes.DimGray;
            public static readonly Thickness GridMargin = new Thickness(10, 5, 10, 5);
            public const int RowHeight = 14;
            
        }

        public List<GridColumnDefinition> ColumnDefinitions { get; set; } = new List<GridColumnDefinition>();

        static ItemListViewGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemListViewGrid),
                new FrameworkPropertyMetadata(typeof(ItemListViewGrid)));
        }

        public ItemListViewGrid()
        {
            _contentPanel = new DockPanel();
            _scrollVertical = new ScrollBar();
            _scrollHorizontal = new ScrollBar();
            _canvas = new Canvas();

            SizeChanged += (x, y) => { RefreshGrid(); };
        }

        public void DefaultInit()
        {
            ColumnDefinitions.Add(new GridColumnDefinition {Width = 100, Title = "Column 1"});
            ColumnDefinitions.Add(new GridColumnDefinition {Width = 200, Title = "Column 2"});
            ColumnDefinitions.Add(new GridColumnDefinition {Width = 150, Title = "Column 3"});
        }

        protected override void OnInitialized(EventArgs e)
        {
            DefaultInit();

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = GridSettings.GridBrush
            };

            //  Create the scroll bars
            _scrollVertical.Orientation = Orientation.Vertical;
            _scrollHorizontal.Orientation = Orientation.Horizontal;
            SetDock(_scrollVertical, Dock.Right);
            SetDock(_scrollHorizontal, Dock.Bottom);
            _contentPanel.Children.Add(_scrollHorizontal);
            _contentPanel.Children.Add(_scrollVertical);

            _canvas.ClipToBounds = true;
            _contentPanel.Children.Add(_canvas);

            border.Child = _contentPanel;

            RefreshGrid();

            Children.Add(border);
        }

        /// <summary>
        /// Gets the cell instantiation of a certain cell and row
        /// </summary>
        /// <param name="row">Row to be evaluated</param>
        /// <param name="column">Column to be evaluation</param>
        /// <returns></returns>
        public CellInstantiation GetCellInstantiation(int row, int column)
        {
            while (_rowInstantiations.Count <= row)
            {
                _rowInstantiations.Add(new RowInstantiation());
            }

            var rowInstantiation = _rowInstantiations[row];

            while (rowInstantiation.Cells.Count <= column)
            {
                rowInstantiation.Cells.Add(new CellInstantiation());
            }

            return rowInstantiation.Cells[column];
        }

        /// <summary>
        /// Gets the column instantiations
        /// </summary>
        /// <param name="column">Number of column</param>
        /// <returns>The retrieved column</returns>
        public ColumnInstantiation GetColumnInstantiation(int column)
        {
            while (_columnInstantiations.Count <= column)
            {
                _columnInstantiations.Add(new ColumnInstantiation());
            }

            return _columnInstantiations[column];
        }

        /// <summary>
        /// Gets the row instantiations
        /// </summary>
        /// <param name="row">Number of row</param>
        /// <returns>The retrieved column</returns>
        public RowInstantiation GetRowInstantiation(int row)
        {
            while (_rowInstantiations.Count <= row)
            {
                _rowInstantiations.Add(new RowInstantiation());
            }

            return _rowInstantiations[row];
        }

        public void UpdateColumnPositions()
        {
            _columnInstantiations.Clear();

            var offsetColumn = GridSettings.GridMargin.Left;
            foreach (var definition in ColumnDefinitions)
            {
                _columnInstantiations.Add(
                    new ColumnInstantiation
                    {
                        PositionOffset = offsetColumn,
                        Width = definition.Width,
                        ColumnDefinition = definition
                    });

                offsetColumn += definition.Width;
                offsetColumn += GridSettings.GridPenWidth
                                + GridSettings.GridMargin.Left
                                + GridSettings.GridMargin.Right;
            }
        }

        public RowInstantiation GetHeaderRowInstantiation()
        {
            var result = new RowInstantiation
            {
                Height = RowHeight,
                BackgroundColor = Brushes.LightGray
            };

            foreach (var column in ColumnDefinitions)
            {
                var cell = new CellInstantiation
                {
                    CellElement = new TextBlock {Text = column.Title}
                };

                result.Cells.Add(cell);
            }

            return result;
        }

        protected void RefreshGrid()
        {
            UpdateColumnPositions();

            _canvas.Children.Clear();
            _rowInstantiations.Clear();

            var positionRow = 0.0;

            var row = 1;
            for (var n = 0; n < RowCount + 1; n++)
            {
                var rowInstantiation = GetVisibleRow(n);
                if (rowInstantiation != null)
                {
                    rowInstantiation.Height = RowHeight;
                    rowInstantiation.PositionOffset = positionRow;
                    _rowInstantiations.Add(rowInstantiation);

                    var c = 0;
                    foreach (var column in rowInstantiation.Cells)
                    {
                        var columnInstantiation = GetColumnInstantiation(c);
                        if (column.CellElement != null)
                        {
                            column.CellElement.Width = columnInstantiation.Width;
                            column.CellElement.ClipToBounds = true;
                            _canvas.Children.Add(column.CellElement);
                        }
                        
                        c++;
                    }
                }

                row++;
                positionRow += RowHeight + GridSettings.GridPenWidth
                                         + GridSettings.GridMargin.Top
                                         + GridSettings.GridMargin.Bottom;

                if (positionRow > _canvas.ActualHeight)
                {
                    break;
                }
            }

            foreach (var rowInstantiation in _rowInstantiations)
            {
                var n = 0;
                foreach (var column in rowInstantiation.Cells)
                {
                    var columnInstantiation = GetColumnInstantiation(n);
                    if (column.CellElement != null)
                    {
                        Canvas.SetTop(column.CellElement, rowInstantiation.PositionOffset);
                        Canvas.SetLeft(column.CellElement, columnInstantiation.PositionOffset);
                    }

                    n++;
                }
            }
            
            InvalidateVisual();
        }

        /// <summary>
        /// Returns the number of columns
        /// </summary>
        public int ColumnCount => ColumnDefinitions.Count;

        /// <summary>
        /// Returns the number of columns
        /// </summary>
        public virtual int RowCount => 10;

        /// <summary>
        /// Gets the row of the content  
        /// </summary>
        /// <param name="row">Number of the row to be queried</param>
        /// <returns>The row instantiation including the content to be shown</returns>
        public virtual RowInstantiation? GetRowOfContent(int row)
        {
            // The instantiated row
            var rowInstantiation = new RowInstantiation();
            var c = 0;
            foreach (var columnInstantiation in _columnInstantiations)
            {
                var cell = new CellInstantiation
                {
                    CellElement = new TextBlock
                    {
                        Text = $"R #{row}, C #{c}",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }
                };

                rowInstantiation.Cells.Add(cell);
                c++;
            }

            return rowInstantiation;
        }

        /// <summary>
        /// Returns the instantiation of the visible row of the content.
        /// The first (0) is usually the header row, then followed by the
        /// conten trows
        /// </summary>
        /// <param name="row">Number of row to be shown</param>
        /// <returns>The instantiated row or null if not available</returns>
        public RowInstantiation? GetVisibleRow(int row)
        {
            bool IsHeaderRow(int theRow) => theRow == 0;
            int MapRows(int theRow) => theRow == 0 ? theRow - 1 : theRow + _rowOffset - 1;

            return IsHeaderRow(row)
                ? GetHeaderRowInstantiation()
                : GetRowOfContent(MapRows(row));
        }

        /// <summary>
        /// Gets the number of real visible rows
        /// </summary>
        /// <returns></returns>
        public int GetVisibleRowCount()
        {
            return RowCount + 1;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var actualWidth = 0.0;
            double ResidualWidth() => _canvas.ActualWidth - actualWidth;

            var blackPen = new Pen(GridSettings.GridBrush, GridSettings.GridPenWidth);

            // Goes through the rows
            var actualHeight = 0.0;
            double ResidualHeight() => _canvas.ActualHeight - actualHeight;
            
            for (var currentRow = 0; currentRow < GetVisibleRowCount(); currentRow++)
            {
                var gridRowDefinition = GetVisibleRow(currentRow);
                if (gridRowDefinition?.BackgroundColor != null)
                {
                    dc.DrawRectangle(
                        gridRowDefinition.BackgroundColor,
                        null, 
                        new Rect(
                            0,
                            actualHeight,
                            ActualWidth,
                            actualHeight
                            + GridSettings.GridMargin.Top
                            + GridSettings.GridMargin.Bottom
                            + gridRowDefinition.Height));
                }

                actualHeight +=
                    gridRowDefinition != null
                        ? gridRowDefinition.Height
                          + GridSettings.GridMargin.Top
                          + GridSettings.GridMargin.Bottom
                          + GridSettings.GridPenWidth
                        : RowHeight;

                dc.DrawLine(
                    blackPen,
                    new Point(0, actualHeight),
                    new Point(_canvas.ActualWidth, actualHeight));

                if (ResidualHeight() < 0)
                {
                    break;
                }
            }

            // Goes through the columns
            for (var currentColumn = _columnOffset; currentColumn < ColumnCount; currentColumn++)
            {
                var columnInstantiation = GetColumnInstantiation(currentColumn);
                actualWidth = columnInstantiation.PositionOffset - GridSettings.GridMargin.Left;

                dc.DrawLine(
                    blackPen,
                    new Point(actualWidth, 0),
                    new Point(actualWidth, _canvas.ActualHeight));

                actualWidth += GridSettings.GridPenWidth;

                if (ResidualWidth() < 0)
                {
                    break;
                }
            }
        }
    }
}
