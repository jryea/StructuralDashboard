let revitLayer, analysisLayer, stage;

const REVIT_COLOR = '#ef4444';
const ANALYSIS_COLOR = '#22C55E';

export function initialize(elementId, dotnetRef) {
  const container = document.getElementById(elementId);
  stage = new Konva.Stage({
    container: elementId,
    width: container.clientWidth,
    height: container.clientHeight,
    draggable: true,
  });

  // Create Layers
  revitLayer = new Konva.Layer();
  analysisLayer = new Konva.Layer();
  stage.add(revitLayer);
  stage.add(analysisLayer);
  addZoom(stage);
}
export function render(canvas) {
    revitLayer.destroyChildren();
    analysisLayer.destroyChildren();

    const { revitModel, analysisModel, revitVisible, analysisVisible } = canvas;

    renderModelToLayer(revitModel, revitLayer, REVIT_COLOR);
    renderModelToLayer(analysisModel, analysisLayer, ANALYSIS_COLOR);

    revitLayer.visible(revitVisible);
    analysisLayer.visible(analysisVisible);

    fitToCanvas(stage, [revitLayer, analysisLayer], [revitModel, analysisModel]);
}

export function destroy(elementId) {
  stage.destroy();
}

function fitToCanvas(stage, layers, models) {
    const present = models.filter(m => m);
    if (present.length === 0) return;

    const minX = Math.min(...present.map(m => m.minX));
    const minY = Math.min(...present.map(m => m.minY));
    const maxX = Math.max(...present.map(m => m.maxX));
    const maxY = Math.max(...present.map(m => m.maxY));

    const padding = 40;
    const modelWidth = maxX - minX;
    const modelHeight = maxY - minY;

    const scaleX = (stage.width() - padding * 2) / modelWidth;
    const scaleY = (stage.height() - padding * 2) / modelHeight;
    const scale = Math.min(scaleX, scaleY);

    const position = {
        x: -minX * scale + padding,
        y: maxY * scale + padding,
    };

    layers.forEach(layer => {
        layer.scale({ x: scale, y: -scale });
        layer.position(position);
    });
}

function renderModelToLayer(model, layer, color) {
  if (!model) return;

  const beams = model.members.filter(
    (x) => x.type === 'beam' || x.type === 'joist',
  );
  const columns = model.members.filter((x) => x.type === 'column');
  const grids = model.members.filter((x) => x.type === 'grid');

  beams.forEach((b) => layer.add(createBeam(b, color)));
  beams.forEach((b) => layer.add(createBeamTag(b, color)));
  columns.forEach((c) => layer.add(createColumn(c, color)));
  grids.forEach((g) => layer.add(createGrid(g, color)));
}

function addZoom(stage) {
  const scaleBy = 1.25;

  stage.on('wheel', (e) => {
    e.evt.preventDefault();

    const oldScale = stage.scaleX();
    const pointer = stage.getPointerPosition();

    const mousePointTo = {
      x: (pointer.x - stage.x()) / oldScale,
      y: (pointer.y - stage.y()) / oldScale,
    };

    const direction = e.evt.deltaY > 0 ? -1 : 1;
    const newScale = direction > 0 ? oldScale * scaleBy : oldScale / scaleBy;

    stage.scale({ x: newScale, y: newScale });

    const newPos = {
      x: pointer.x - mousePointTo.x * newScale,
      y: pointer.y - mousePointTo.y * newScale,
    };

    stage.position(newPos);
    stage.batchDraw();
  });
}

function createBeam(beam, color) {
  return new Konva.Line({
    points: [beam.x1, beam.y1, beam.x2, beam.y2],
    stroke: color,
    strokeWidth: beam.type == 'beam' ? 4 : 2,
    opacity: 0.7,
  });
}

function createBeamTag(beam, color) {
  const tag = new Konva.Text({
    x: beam.tagX,
    y: beam.tagY,
    text: beam.label,
    rotation: beam.tagRotation,
    fontSize: 14,
    scaleX: -1,
    fill: color,
    opacity: 1,
  });

  tag.offsetX(tag.width() / 2);
  return tag;
}

function createColumn(column, color) {
  const rect = new Konva.Rect({
    x: column.x1 - 4,
    y: column.y1 - 4,
    width: 8,
    height: 8,
    stroke: color,
    strokeWidth: 2,
    opacity: 0.7,
    rotation: column.rotation,
  });
  return rect;
}

function createGrid(grid, color) {
  const line = new Konva.Line({
    points: [grid.x1, grid.y1, grid.x2, grid.y2],
    stroke: color,
    strokeWidth: 1,
    dash: [4, 4],
    opacity: 0.7,
  });

  const bubbleStart = new Konva.Circle({
    x: grid.x1,
    y: grid.y1,
    radius: 24,
    stroke: color,
    strokeWidth: 1,
    opacity: 0.7,
  });
  const bubbleEnd = new Konva.Circle({
    x: grid.x2,
    y: grid.y2,
    radius: 24,
    stroke: color,
    strokeWidth: 1,
    opacity: 0.7,
  });
  const gridBubbleTextStart = new Konva.Text({
    x: grid.x1,
    y: grid.y1,
    text: grid.label,
    fontSize: 14,
    fill: color,
    scaleY: -1,
  });
  const gridBubbleTextEnd = new Konva.Text({
    x: grid.x2,
    y: grid.y2,
    text: grid.label,
    fontSize: 14,
    fill: color,
    scaleY: -1,
  });

  gridBubbleTextStart.offsetX(gridBubbleTextStart.width() / 2);
  gridBubbleTextStart.offsetY(gridBubbleTextStart.height() / 2);
  gridBubbleTextEnd.offsetX(gridBubbleTextEnd.width() / 2);
  gridBubbleTextEnd.offsetY(gridBubbleTextEnd.height() / 2);

  const group = new Konva.Group();
  group.add(line);
  group.add(bubbleStart);
  group.add(bubbleEnd);
  group.add(gridBubbleTextStart);
  group.add(gridBubbleTextEnd);

  return group;
}
