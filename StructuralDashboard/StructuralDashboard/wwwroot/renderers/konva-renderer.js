let revitLayer, analysisLayer, stage;

const REVIT_COLOR = '#ef4444';
const ANALYSIS_COLOR = '#22C55E';
const MATCH_COLOR = '#FFFFFF';
const SIZE_COLOR = '#FFFF00';

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

    const { revitModel, analysisModel, revitVisible, analysisVisible, analysisTransform } = canvas;

    renderModelToLayer(revitModel, revitLayer, REVIT_COLOR, null);
    renderModelToLayer(analysisModel, analysisLayer, ANALYSIS_COLOR, analysisTransform);

    revitLayer.visible(revitVisible);
    analysisLayer.visible(analysisVisible);

    fitToCanvas(stage, [revitLayer, analysisLayer], [
        { model: revitModel, transform: null },
        { model: analysisModel, transform: analysisTransform },
    ]);
}

export function destroy(elementId) {
  stage.destroy();
}

function fitToCanvas(stage, layers, entries) {
    const boxes = entries
        .filter(e => e.model)
        .map(e => transformBounds(e.model, e.transform));
    if (boxes.length === 0) return;

    const minX = Math.min(...boxes.map(b => b.minX));
    const minY = Math.min(...boxes.map(b => b.minY));
    const maxX = Math.max(...boxes.map(b => b.maxX));
    const maxY = Math.max(...boxes.map(b => b.maxY));

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

function transformBounds(model, t) {
    const corners = [
        applyTransform(model.minX, model.minY, t),
        applyTransform(model.maxX, model.minY, t),
        applyTransform(model.minX, model.maxY, t),
        applyTransform(model.maxX, model.maxY, t),
    ];
    return {
        minX: Math.min(...corners.map(p => p.x)),
        minY: Math.min(...corners.map(p => p.y)),
        maxX: Math.max(...corners.map(p => p.x)),
        maxY: Math.max(...corners.map(p => p.y)),
    };
}

function applyTransform(x, y, t) {
    if (!t) return { x, y };
    const rad = ((t.rotation || 0) * Math.PI) / 180;
    const c = Math.cos(rad);
    const s = Math.sin(rad);
    return {
        x: x * c - y * s + (t.x || 0),
        y: x * s + y * c + (t.y || 0),
    };
}

function strokeFor(member, sourceColor) {
    switch (member.matchStatus) {
        case 'Match':  return MATCH_COLOR;
        case 'Size':   return SIZE_COLOR;
        case 'R Only': return REVIT_COLOR;
        case 'A Only': return ANALYSIS_COLOR;
        default:       return sourceColor;
    }
}

function renderModelToLayer(model, layer, color, transform) {
  if (!model) return;

  const beams = model.members.filter(
    (x) => x.type === 'beam' || x.type === 'joist',
  );
  const columns = model.members.filter((x) => x.type === 'column');
  const grids = model.members.filter((x) => x.type === 'grid');

  beams.forEach((b) => layer.add(createBeam(b, color, transform)));
  beams.forEach((b) => layer.add(createBeamTag(b, color, transform)));
  columns.forEach((c) => layer.add(createColumn(c, color, transform)));
  grids.forEach((g) => layer.add(createGrid(g, color, transform)));
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

function createBeam(beam, color, transform) {
  const p1 = applyTransform(beam.x1, beam.y1, transform);
  const p2 = applyTransform(beam.x2, beam.y2, transform);
  return new Konva.Line({
    points: [p1.x, p1.y, p2.x, p2.y],
    stroke: strokeFor(beam, color),
    strokeWidth: beam.type == 'beam' ? 4 : 2,
    opacity: 0.7,
  });
}

function createBeamTag(beam, color, transform) {
  const p = applyTransform(beam.tagX, beam.tagY, transform);
  const extraRotation = transform ? (transform.rotation || 0) : 0;
  const tag = new Konva.Text({
    x: p.x,
    y: p.y,
    text: beam.label,
    rotation: beam.tagRotation + extraRotation,
    fontSize: 14,
    scaleX: -1,
    fill: strokeFor(beam, color),
    opacity: 1,
  });

  tag.offsetX(tag.width() / 2);
  return tag;
}

function createColumn(column, color, transform) {
  const p = applyTransform(column.x1, column.y1, transform);
  const extraRotation = transform ? (transform.rotation || 0) : 0;
  const rect = new Konva.Rect({
    x: p.x - 4,
    y: p.y - 4,
    width: 8,
    height: 8,
    stroke: color,
    strokeWidth: 2,
    opacity: 0.7,
    rotation: extraRotation,
  });
  return rect;
}

function createGrid(grid, color, transform) {
  const p1 = applyTransform(grid.x1, grid.y1, transform);
  const p2 = applyTransform(grid.x2, grid.y2, transform);
  const stroke = strokeFor(grid, color);

  const line = new Konva.Line({
    points: [p1.x, p1.y, p2.x, p2.y],
    stroke: stroke,
    strokeWidth: 1,
    dash: [4, 4],
    opacity: 0.7,
  });

  const bubbleStart = new Konva.Circle({
    x: p1.x,
    y: p1.y,
    radius: 24,
    stroke: stroke,
    strokeWidth: 1,
    opacity: 0.7,
  });
  const bubbleEnd = new Konva.Circle({
    x: p2.x,
    y: p2.y,
    radius: 24,
    stroke: stroke,
    strokeWidth: 1,
    opacity: 0.7,
  });
  const gridBubbleTextStart = new Konva.Text({
    x: p1.x,
    y: p1.y,
    text: grid.label,
    fontSize: 14,
    fill: stroke,
    scaleY: -1,
  });
  const gridBubbleTextEnd = new Konva.Text({
    x: p2.x,
    y: p2.y,
    text: grid.label,
    fontSize: 14,
    fill: stroke,
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
