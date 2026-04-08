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
  stage.add(layer);
}

export function render(model) {
  const beams = model.members.filter((x) => x.type === 'beam');
  const beamLines = beams.map((b) => {
    return new Konva.Line({
      points: [b.x1, b.y1, b.x2, b.y2],
      stroke: 'white',
      strokeWidth: 6,
      opacity: 0.7,
    });
  });

  beamLines.map((b) => layer.add(b));

  fitToCanvas(model);

  addZoom(stage);
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
