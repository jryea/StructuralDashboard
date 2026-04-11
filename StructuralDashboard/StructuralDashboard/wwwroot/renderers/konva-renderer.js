let stage, layer;

export function initialize(elementId, model) {
  // Create a stage (container for all layers)
  stage = new Konva.Stage({
    container: elementId,
    width: 1600,
    height: 800,
    draggable: true,
  });

  // create a layer and add it
  layer = new Konva.Layer();
  layer.clearBeforeDraw(true);
  stage.add(layer);

  addZoom(stage);
}

export function render(model) {
  layer.destroyChildren();

  const beams = model.members.filter(
    (x) => x.type === 'beam' || x.type === 'joist',
  );
  const columns = model.members.filter((x) => x.type === 'column');

  const beamLines = beams.map((b) => {
    return new Konva.Line({
      points: [b.x1, b.y1, b.x2, b.y2],
      stroke: 'white',
      strokeWidth: b.type == 'beam' ? 4 : 2,
      opacity: 0.7,
    });
  });

  const beamTags = beams.map((b) => createBeamTag(b));

  beamLines.forEach((b) => layer.add(b));
  beamTags.forEach((b) => layer.add(b));

  fitToCanvas(model);
}

export function destroy(elementId) {
  console.log('renderer detroyed', elementId);
}

function fitToCanvas(model) {
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

function createBeamTag(beam) {
  const tag = new Konva.Text({
    x: beam.tagX,
    y: beam.tagY,
    text: beam.size,
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
    x: column.x1,
    y: column.y1,
    width: 8,
    height: 8,
    stroke: 'white',
    strokeWidth: 2,
    opacity: 0.7,
    rotation: column.rotation,
  });

  return rect;
}
