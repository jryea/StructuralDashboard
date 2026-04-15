const stages = {};
const layers = {};

export function initialize(elementId, dotnetRef) {
  // Create a stage (container for all layers)
  stages[elementId] = new Konva.Stage({
    container: elementId,
    width: 1600,
    height: 800,
    draggable: true,
  });

  // create a layer and add it
  layers[elementId] = new Konva.Layer();
  stages[elementId].add(layers[elementId]);

  addZoom(stages[elementId]);
}

export function render(elementId, model) {
  const layer = layers[elementId];
  const stage = stages[elementId];
  layer.destroyChildren();

  const beams = model.members.filter(
    (x) => x.type === 'beam' || x.type === 'joist',
  );
  const columns = model.members.filter((x) => x.type === 'column');
  const grids = model.members.filter((x) => x.type === 'grid');

  const beamLines = beams.map((b) => createBeam(b));
  const columnRects = columns.map((c) => createColumn(c));
  const beamTags = beams.map((b) => createBeamTag(b));
  const gridLines = grids.map((g) => createGrid(g));

  beamLines.forEach((b) => layer.add(b));
  beamTags.forEach((b) => layer.add(b));
  columnRects.forEach((c) => layer.add(c));
  gridLines.forEach((g) => layer.add(g));

  fitToCanvas(stage, layer, model);
}

export function destroy(elementId) {
  stages[elementId]?.destroy();
  delete stages[elementId];
  delete layers[elementId];
}

function fitToCanvas(stage, layer, model) {
  const stageWidth = stage.width();
  const stageHeight = stage.height();

  const padding = 40;
  const modelWidth = model.maxX - model.minX;
  const modelHeight = model.maxY - model.minY;

  const scaleX = (stage.width() - padding * 2) / modelWidth;
  const scaleY = (stage.height() - padding * 2) / modelHeight;
  const scale = Math.min(scaleX, scaleY);

  layer.scale({ x: scale, y: -scale });
  layer.position({
    x: -model.minX * scale + padding,
    y: model.maxY * scale + padding,
  });
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

function createBeam(beam) {
  return new Konva.Line({
    points: [beam.x1, beam.y1, beam.x2, beam.y2],
    stroke: 'white',
    strokeWidth: beam.type == 'beam' ? 4 : 2,
    opacity: 0.7,
  });
}

function createBeamTag(beam) {
  const tag = new Konva.Text({
    x: beam.tagX,
    y: beam.tagY,
    text: beam.label,
    rotation: beam.tagRotation,
    fontSize: 14,
    scaleX: -1,
    fill: 'white',
    opacity: 1,
  });

  tag.offsetX(tag.width() / 2);
  return tag;
}

function createColumn(column) {
  const rect = new Konva.Rect({
    x: column.x1 - 4,
    y: column.y1 - 4,
    width: 8,
    height: 8,
    stroke: 'white',
    strokeWidth: 2,
    opacity: 0.7,
    rotation: column.rotation,
  });
  return rect;
}

function createGrid(grid) {
  const line = new Konva.Line({
    points: [grid.x1, grid.y1, grid.x2, grid.y2],
    stroke: 'white',
    strokeWidth: 1,
    dash: [4, 4],
    opacity: 0.7,
  });

  const bubbleStart = new Konva.Circle({
    x: grid.x1,
    y: grid.y1,
    radius: 24,
    stroke: 'white',
    strokeWidth: 1,
    opacity: 0.7,
  });
  const bubbleEnd = new Konva.Circle({
    x: grid.x2,
    y: grid.y2,
    radius: 24,
    stroke: 'white',
    strokeWidth: 1,
    opacity: 0.7,
  });
  const gridBubbleTextStart = new Konva.Text({
    x: grid.x1,
    y: grid.y1,
    text: grid.label,
    fontSize: 14,
    fill: 'white',
    scaleY: -1,
  });
  const gridBubbleTextEnd = new Konva.Text({
    x: grid.x2,
    y: grid.y2,
    text: grid.label,
    fontSize: 14,
    fill: 'white',
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
