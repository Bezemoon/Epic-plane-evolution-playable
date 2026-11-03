var Deserializers = {}
Deserializers["UnityEngine.JointSpring"] = function (request, data, root) {
  var i242 = root || request.c( 'UnityEngine.JointSpring' )
  var i243 = data
  i242.spring = i243[0]
  i242.damper = i243[1]
  i242.targetPosition = i243[2]
  return i242
}

Deserializers["UnityEngine.JointMotor"] = function (request, data, root) {
  var i244 = root || request.c( 'UnityEngine.JointMotor' )
  var i245 = data
  i244.m_TargetVelocity = i245[0]
  i244.m_Force = i245[1]
  i244.m_FreeSpin = i245[2]
  return i244
}

Deserializers["UnityEngine.JointLimits"] = function (request, data, root) {
  var i246 = root || request.c( 'UnityEngine.JointLimits' )
  var i247 = data
  i246.m_Min = i247[0]
  i246.m_Max = i247[1]
  i246.m_Bounciness = i247[2]
  i246.m_BounceMinVelocity = i247[3]
  i246.m_ContactDistance = i247[4]
  i246.minBounce = i247[5]
  i246.maxBounce = i247[6]
  return i246
}

Deserializers["UnityEngine.JointDrive"] = function (request, data, root) {
  var i248 = root || request.c( 'UnityEngine.JointDrive' )
  var i249 = data
  i248.m_PositionSpring = i249[0]
  i248.m_PositionDamper = i249[1]
  i248.m_MaximumForce = i249[2]
  i248.m_UseAcceleration = i249[3]
  return i248
}

Deserializers["UnityEngine.SoftJointLimitSpring"] = function (request, data, root) {
  var i250 = root || request.c( 'UnityEngine.SoftJointLimitSpring' )
  var i251 = data
  i250.m_Spring = i251[0]
  i250.m_Damper = i251[1]
  return i250
}

Deserializers["UnityEngine.SoftJointLimit"] = function (request, data, root) {
  var i252 = root || request.c( 'UnityEngine.SoftJointLimit' )
  var i253 = data
  i252.m_Limit = i253[0]
  i252.m_Bounciness = i253[1]
  i252.m_ContactDistance = i253[2]
  return i252
}

Deserializers["UnityEngine.WheelFrictionCurve"] = function (request, data, root) {
  var i254 = root || request.c( 'UnityEngine.WheelFrictionCurve' )
  var i255 = data
  i254.m_ExtremumSlip = i255[0]
  i254.m_ExtremumValue = i255[1]
  i254.m_AsymptoteSlip = i255[2]
  i254.m_AsymptoteValue = i255[3]
  i254.m_Stiffness = i255[4]
  return i254
}

Deserializers["UnityEngine.JointAngleLimits2D"] = function (request, data, root) {
  var i256 = root || request.c( 'UnityEngine.JointAngleLimits2D' )
  var i257 = data
  i256.m_LowerAngle = i257[0]
  i256.m_UpperAngle = i257[1]
  return i256
}

Deserializers["UnityEngine.JointMotor2D"] = function (request, data, root) {
  var i258 = root || request.c( 'UnityEngine.JointMotor2D' )
  var i259 = data
  i258.m_MotorSpeed = i259[0]
  i258.m_MaximumMotorTorque = i259[1]
  return i258
}

Deserializers["UnityEngine.JointSuspension2D"] = function (request, data, root) {
  var i260 = root || request.c( 'UnityEngine.JointSuspension2D' )
  var i261 = data
  i260.m_DampingRatio = i261[0]
  i260.m_Frequency = i261[1]
  i260.m_Angle = i261[2]
  return i260
}

Deserializers["UnityEngine.JointTranslationLimits2D"] = function (request, data, root) {
  var i262 = root || request.c( 'UnityEngine.JointTranslationLimits2D' )
  var i263 = data
  i262.m_LowerTranslation = i263[0]
  i262.m_UpperTranslation = i263[1]
  return i262
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh"] = function (request, data, root) {
  var i264 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh' )
  var i265 = data
  i264.name = i265[0]
  i264.halfPrecision = !!i265[1]
  i264.useSimplification = !!i265[2]
  i264.useUInt32IndexFormat = !!i265[3]
  i264.vertexCount = i265[4]
  i264.aabb = i265[5]
  var i267 = i265[6]
  var i266 = []
  for(var i = 0; i < i267.length; i += 1) {
    i266.push( !!i267[i + 0] );
  }
  i264.streams = i266
  i264.vertices = i265[7]
  var i269 = i265[8]
  var i268 = []
  for(var i = 0; i < i269.length; i += 1) {
    i268.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh', i269[i + 0]) );
  }
  i264.subMeshes = i268
  var i271 = i265[9]
  var i270 = []
  for(var i = 0; i < i271.length; i += 16) {
    i270.push( new pc.Mat4().setData(i271[i + 0], i271[i + 1], i271[i + 2], i271[i + 3],  i271[i + 4], i271[i + 5], i271[i + 6], i271[i + 7],  i271[i + 8], i271[i + 9], i271[i + 10], i271[i + 11],  i271[i + 12], i271[i + 13], i271[i + 14], i271[i + 15]) );
  }
  i264.bindposes = i270
  var i273 = i265[10]
  var i272 = []
  for(var i = 0; i < i273.length; i += 1) {
    i272.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape', i273[i + 0]) );
  }
  i264.blendShapes = i272
  return i264
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh"] = function (request, data, root) {
  var i278 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh' )
  var i279 = data
  i278.triangles = i279[0]
  return i278
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape"] = function (request, data, root) {
  var i284 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape' )
  var i285 = data
  i284.name = i285[0]
  var i287 = i285[1]
  var i286 = []
  for(var i = 0; i < i287.length; i += 1) {
    i286.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame', i287[i + 0]) );
  }
  i284.frames = i286
  return i284
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material"] = function (request, data, root) {
  var i288 = root || new pc.UnityMaterial()
  var i289 = data
  i288.name = i289[0]
  request.r(i289[1], i289[2], 0, i288, 'shader')
  i288.renderQueue = i289[3]
  i288.enableInstancing = !!i289[4]
  var i291 = i289[5]
  var i290 = []
  for(var i = 0; i < i291.length; i += 1) {
    i290.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter', i291[i + 0]) );
  }
  i288.floatParameters = i290
  var i293 = i289[6]
  var i292 = []
  for(var i = 0; i < i293.length; i += 1) {
    i292.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter', i293[i + 0]) );
  }
  i288.colorParameters = i292
  var i295 = i289[7]
  var i294 = []
  for(var i = 0; i < i295.length; i += 1) {
    i294.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter', i295[i + 0]) );
  }
  i288.vectorParameters = i294
  var i297 = i289[8]
  var i296 = []
  for(var i = 0; i < i297.length; i += 1) {
    i296.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter', i297[i + 0]) );
  }
  i288.textureParameters = i296
  var i299 = i289[9]
  var i298 = []
  for(var i = 0; i < i299.length; i += 1) {
    i298.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag', i299[i + 0]) );
  }
  i288.materialFlags = i298
  return i288
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter"] = function (request, data, root) {
  var i302 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter' )
  var i303 = data
  i302.name = i303[0]
  i302.value = i303[1]
  return i302
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter"] = function (request, data, root) {
  var i306 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter' )
  var i307 = data
  i306.name = i307[0]
  i306.value = new pc.Color(i307[1], i307[2], i307[3], i307[4])
  return i306
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter"] = function (request, data, root) {
  var i310 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter' )
  var i311 = data
  i310.name = i311[0]
  i310.value = new pc.Vec4( i311[1], i311[2], i311[3], i311[4] )
  return i310
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter"] = function (request, data, root) {
  var i314 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter' )
  var i315 = data
  i314.name = i315[0]
  request.r(i315[1], i315[2], 0, i314, 'value')
  return i314
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag"] = function (request, data, root) {
  var i318 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag' )
  var i319 = data
  i318.name = i319[0]
  i318.enabled = !!i319[1]
  return i318
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Texture2D"] = function (request, data, root) {
  var i320 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Texture2D' )
  var i321 = data
  i320.name = i321[0]
  i320.width = i321[1]
  i320.height = i321[2]
  i320.mipmapCount = i321[3]
  i320.anisoLevel = i321[4]
  i320.filterMode = i321[5]
  i320.hdr = !!i321[6]
  i320.format = i321[7]
  i320.wrapMode = i321[8]
  i320.alphaIsTransparency = !!i321[9]
  i320.alphaSource = i321[10]
  i320.graphicsFormat = i321[11]
  i320.sRGBTexture = !!i321[12]
  i320.desiredColorSpace = i321[13]
  i320.wrapU = i321[14]
  i320.wrapV = i321[15]
  return i320
}

Deserializers["Luna.Unity.DTO.UnityEngine.Textures.Cubemap"] = function (request, data, root) {
  var i322 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Textures.Cubemap' )
  var i323 = data
  i322.name = i323[0]
  i322.atlasId = i323[1]
  i322.mipmapCount = i323[2]
  i322.hdr = !!i323[3]
  i322.size = i323[4]
  i322.anisoLevel = i323[5]
  i322.filterMode = i323[6]
  var i325 = i323[7]
  var i324 = []
  for(var i = 0; i < i325.length; i += 4) {
    i324.push( UnityEngine.Rect.MinMaxRect(i325[i + 0], i325[i + 1], i325[i + 2], i325[i + 3]) );
  }
  i322.rects = i324
  i322.wrapU = i323[8]
  i322.wrapV = i323[9]
  return i322
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.Scene"] = function (request, data, root) {
  var i328 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.Scene' )
  var i329 = data
  i328.name = i329[0]
  i328.index = i329[1]
  i328.startup = !!i329[2]
  return i328
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Camera"] = function (request, data, root) {
  var i330 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Camera' )
  var i331 = data
  i330.aspect = i331[0]
  i330.orthographic = !!i331[1]
  i330.orthographicSize = i331[2]
  i330.backgroundColor = new pc.Color(i331[3], i331[4], i331[5], i331[6])
  i330.nearClipPlane = i331[7]
  i330.farClipPlane = i331[8]
  i330.fieldOfView = i331[9]
  i330.depth = i331[10]
  i330.clearFlags = i331[11]
  i330.cullingMask = i331[12]
  i330.rect = i331[13]
  request.r(i331[14], i331[15], 0, i330, 'targetTexture')
  i330.usePhysicalProperties = !!i331[16]
  i330.focalLength = i331[17]
  i330.sensorSize = new pc.Vec2( i331[18], i331[19] )
  i330.lensShift = new pc.Vec2( i331[20], i331[21] )
  i330.gateFit = i331[22]
  i330.commandBufferCount = i331[23]
  i330.cameraType = i331[24]
  i330.enabled = !!i331[25]
  return i330
}

Deserializers["Luna.Unity.DTO.UnityEngine.Scene.GameObject"] = function (request, data, root) {
  var i332 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Scene.GameObject' )
  var i333 = data
  i332.name = i333[0]
  i332.tagId = i333[1]
  i332.enabled = !!i333[2]
  i332.isStatic = !!i333[3]
  i332.layer = i333[4]
  return i332
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.Light"] = function (request, data, root) {
  var i334 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.Light' )
  var i335 = data
  i334.type = i335[0]
  i334.color = new pc.Color(i335[1], i335[2], i335[3], i335[4])
  i334.cullingMask = i335[5]
  i334.intensity = i335[6]
  i334.range = i335[7]
  i334.spotAngle = i335[8]
  i334.shadows = i335[9]
  i334.shadowNormalBias = i335[10]
  i334.shadowBias = i335[11]
  i334.shadowStrength = i335[12]
  i334.shadowResolution = i335[13]
  i334.lightmapBakeType = i335[14]
  i334.renderMode = i335[15]
  request.r(i335[16], i335[17], 0, i334, 'cookie')
  i334.cookieSize = i335[18]
  i334.shadowNearPlane = i335[19]
  i334.occlusionMaskChannel = i335[20]
  i334.isBaked = !!i335[21]
  i334.mixedLightingMode = i335[22]
  i334.enabled = !!i335[23]
  return i334
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshFilter"] = function (request, data, root) {
  var i336 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshFilter' )
  var i337 = data
  request.r(i337[0], i337[1], 0, i336, 'sharedMesh')
  return i336
}

Deserializers["Luna.Unity.DTO.UnityEngine.Components.MeshRenderer"] = function (request, data, root) {
  var i338 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Components.MeshRenderer' )
  var i339 = data
  request.r(i339[0], i339[1], 0, i338, 'additionalVertexStreams')
  i338.enabled = !!i339[2]
  request.r(i339[3], i339[4], 0, i338, 'sharedMaterial')
  var i341 = i339[5]
  var i340 = []
  for(var i = 0; i < i341.length; i += 2) {
  request.r(i341[i + 0], i341[i + 1], 2, i340, '')
  }
  i338.sharedMaterials = i340
  i338.receiveShadows = !!i339[6]
  i338.shadowCastingMode = i339[7]
  i338.sortingLayerID = i339[8]
  i338.sortingOrder = i339[9]
  i338.lightmapIndex = i339[10]
  i338.lightmapSceneIndex = i339[11]
  i338.lightmapScaleOffset = new pc.Vec4( i339[12], i339[13], i339[14], i339[15] )
  i338.lightProbeUsage = i339[16]
  i338.reflectionProbeUsage = i339[17]
  return i338
}

Deserializers["NewPlaneBehaviour.Plane"] = function (request, data, root) {
  var i344 = root || request.c( 'NewPlaneBehaviour.Plane' )
  var i345 = data
  request.r(i345[0], i345[1], 0, i344, '_planeRoot')
  request.r(i345[2], i345[3], 0, i344, '_cameraTransform')
  request.r(i345[4], i345[5], 0, i344, '_camera')
  request.r(i345[6], i345[7], 0, i344, '_rampStartPoint')
  request.r(i345[8], i345[9], 0, i344, '_rampEndPoint')
  request.r(i345[10], i345[11], 0, i344, '_rampControlPoint')
  i344._climbSpeed = i345[12]
  i344._climbAngle = i345[13]
  i344._climbDuration = i345[14]
  i344._noseTurnDuration = i345[15]
  i344._climbSpeedCurve = new pc.AnimationCurve( { keys_flow: i345[16] } )
  i344._gravity = i345[17]
  i344._groundHeight = i345[18]
  i344._maxHeight = i345[19]
  i344._drag = i345[20]
  i344._climbCost = i345[21]
  i344._noControlSpeed = i345[22]
  i344._fullControlSpeed = i345[23]
  i344._pitchRate = i345[24]
  i344._turnRate = i345[25]
  i344._maxTurnAngle = i345[26]
  i344._maxWingAngle = i345[27]
  i344._wingReactionSpeed = i345[28]
  i344._maxSideDistance = i345[29]
  i344._cameraOffset = new pc.Vec3( i345[30], i345[31], i345[32] )
  i344._cameraFollowSharpness = i345[33]
  i344._cameraLookSharpness = i345[34]
  i344._cameraLookAheadDistance = i345[35]
  i344._joystickRadius = i345[36]
  i344._maxPullDistance = i345[37]
  i344._grabRadius = i345[38]
  i344._maxNoseRaiseAngle = i345[39]
  i344._pullReactionSpeed = i345[40]
  i344._minPullRadius = i345[41]
  i344._maxPullRadius = i345[42]
  i344._minLaunchSpeed = i345[43]
  i344._maxLaunchSpeed = i345[44]
  return i344
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings"] = function (request, data, root) {
  var i346 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings' )
  var i347 = data
  i346.ambientIntensity = i347[0]
  i346.reflectionIntensity = i347[1]
  i346.ambientMode = i347[2]
  i346.ambientLight = new pc.Color(i347[3], i347[4], i347[5], i347[6])
  i346.ambientSkyColor = new pc.Color(i347[7], i347[8], i347[9], i347[10])
  i346.ambientGroundColor = new pc.Color(i347[11], i347[12], i347[13], i347[14])
  i346.ambientEquatorColor = new pc.Color(i347[15], i347[16], i347[17], i347[18])
  i346.fogColor = new pc.Color(i347[19], i347[20], i347[21], i347[22])
  i346.fogEndDistance = i347[23]
  i346.fogStartDistance = i347[24]
  i346.fogDensity = i347[25]
  i346.fog = !!i347[26]
  request.r(i347[27], i347[28], 0, i346, 'skybox')
  i346.fogMode = i347[29]
  var i349 = i347[30]
  var i348 = []
  for(var i = 0; i < i349.length; i += 1) {
    i348.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap', i349[i + 0]) );
  }
  i346.lightmaps = i348
  i346.lightProbes = request.d('Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes', i347[31], i346.lightProbes)
  i346.lightmapsMode = i347[32]
  i346.mixedBakeMode = i347[33]
  i346.environmentLightingMode = i347[34]
  i346.ambientProbe = new pc.SphericalHarmonicsL2(i347[35])
  request.r(i347[36], i347[37], 0, i346, 'customReflection')
  request.r(i347[38], i347[39], 0, i346, 'defaultReflection')
  i346.defaultReflectionMode = i347[40]
  i346.defaultReflectionResolution = i347[41]
  i346.sunLightObjectId = i347[42]
  i346.pixelLightCount = i347[43]
  i346.defaultReflectionHDR = !!i347[44]
  i346.hasLightDataAsset = !!i347[45]
  i346.hasManualGenerate = !!i347[46]
  return i346
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap"] = function (request, data, root) {
  var i352 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap' )
  var i353 = data
  request.r(i353[0], i353[1], 0, i352, 'lightmapColor')
  request.r(i353[2], i353[3], 0, i352, 'lightmapDirection')
  request.r(i353[4], i353[5], 0, i352, 'shadowMask')
  return i352
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes"] = function (request, data, root) {
  var i354 = root || new UnityEngine.LightProbes()
  var i355 = data
  return i354
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader"] = function (request, data, root) {
  var i362 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader' )
  var i363 = data
  var i365 = i363[0]
  var i364 = new (System.Collections.Generic.List$1(Bridge.ns('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError')))
  for(var i = 0; i < i365.length; i += 1) {
    i364.add(request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError', i365[i + 0]));
  }
  i362.ShaderCompilationErrors = i364
  i362.name = i363[1]
  i362.guid = i363[2]
  var i367 = i363[3]
  var i366 = []
  for(var i = 0; i < i367.length; i += 1) {
    i366.push( i367[i + 0] );
  }
  i362.shaderDefinedKeywords = i366
  var i369 = i363[4]
  var i368 = []
  for(var i = 0; i < i369.length; i += 1) {
    i368.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass', i369[i + 0]) );
  }
  i362.passes = i368
  var i371 = i363[5]
  var i370 = []
  for(var i = 0; i < i371.length; i += 1) {
    i370.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass', i371[i + 0]) );
  }
  i362.usePasses = i370
  var i373 = i363[6]
  var i372 = []
  for(var i = 0; i < i373.length; i += 1) {
    i372.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue', i373[i + 0]) );
  }
  i362.defaultParameterValues = i372
  request.r(i363[7], i363[8], 0, i362, 'unityFallbackShader')
  i362.readDepth = !!i363[9]
  i362.hasDepthOnlyPass = !!i363[10]
  i362.isCreatedByShaderGraph = !!i363[11]
  i362.disableBatching = !!i363[12]
  i362.compiled = !!i363[13]
  return i362
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError"] = function (request, data, root) {
  var i376 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError' )
  var i377 = data
  i376.shaderName = i377[0]
  i376.errorMessage = i377[1]
  return i376
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass"] = function (request, data, root) {
  var i382 = root || new pc.UnityShaderPass()
  var i383 = data
  i382.id = i383[0]
  i382.subShaderIndex = i383[1]
  i382.name = i383[2]
  i382.passType = i383[3]
  i382.grabPassTextureName = i383[4]
  i382.usePass = !!i383[5]
  i382.zTest = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[6], i382.zTest)
  i382.zWrite = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[7], i382.zWrite)
  i382.culling = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[8], i382.culling)
  i382.blending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i383[9], i382.blending)
  i382.alphaBlending = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending', i383[10], i382.alphaBlending)
  i382.colorWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[11], i382.colorWriteMask)
  i382.offsetUnits = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[12], i382.offsetUnits)
  i382.offsetFactor = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[13], i382.offsetFactor)
  i382.stencilRef = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[14], i382.stencilRef)
  i382.stencilReadMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[15], i382.stencilReadMask)
  i382.stencilWriteMask = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i383[16], i382.stencilWriteMask)
  i382.stencilOp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i383[17], i382.stencilOp)
  i382.stencilOpFront = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i383[18], i382.stencilOpFront)
  i382.stencilOpBack = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp', i383[19], i382.stencilOpBack)
  var i385 = i383[20]
  var i384 = []
  for(var i = 0; i < i385.length; i += 1) {
    i384.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag', i385[i + 0]) );
  }
  i382.tags = i384
  var i387 = i383[21]
  var i386 = []
  for(var i = 0; i < i387.length; i += 1) {
    i386.push( i387[i + 0] );
  }
  i382.passDefinedKeywords = i386
  var i389 = i383[22]
  var i388 = []
  for(var i = 0; i < i389.length; i += 1) {
    i388.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup', i389[i + 0]) );
  }
  i382.passDefinedKeywordGroups = i388
  var i391 = i383[23]
  var i390 = []
  for(var i = 0; i < i391.length; i += 1) {
    i390.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i391[i + 0]) );
  }
  i382.variants = i390
  var i393 = i383[24]
  var i392 = []
  for(var i = 0; i < i393.length; i += 1) {
    i392.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant', i393[i + 0]) );
  }
  i382.excludedVariants = i392
  i382.hasDepthReader = !!i383[25]
  return i382
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value"] = function (request, data, root) {
  var i394 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value' )
  var i395 = data
  i394.val = i395[0]
  i394.name = i395[1]
  return i394
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending"] = function (request, data, root) {
  var i396 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending' )
  var i397 = data
  i396.src = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i397[0], i396.src)
  i396.dst = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i397[1], i396.dst)
  i396.op = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i397[2], i396.op)
  return i396
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp"] = function (request, data, root) {
  var i398 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp' )
  var i399 = data
  i398.pass = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i399[0], i398.pass)
  i398.fail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i399[1], i398.fail)
  i398.zFail = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i399[2], i398.zFail)
  i398.comp = request.d('Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value', i399[3], i398.comp)
  return i398
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag"] = function (request, data, root) {
  var i402 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag' )
  var i403 = data
  i402.name = i403[0]
  i402.value = i403[1]
  return i402
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup"] = function (request, data, root) {
  var i406 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup' )
  var i407 = data
  var i409 = i407[0]
  var i408 = []
  for(var i = 0; i < i409.length; i += 1) {
    i408.push( i409[i + 0] );
  }
  i406.keywords = i408
  i406.hasDiscard = !!i407[1]
  return i406
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant"] = function (request, data, root) {
  var i412 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant' )
  var i413 = data
  i412.passId = i413[0]
  i412.subShaderIndex = i413[1]
  var i415 = i413[2]
  var i414 = []
  for(var i = 0; i < i415.length; i += 1) {
    i414.push( i415[i + 0] );
  }
  i412.keywords = i414
  i412.vertexProgram = i413[3]
  i412.fragmentProgram = i413[4]
  i412.exportedForWebGl2 = !!i413[5]
  i412.readDepth = !!i413[6]
  return i412
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass"] = function (request, data, root) {
  var i418 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass' )
  var i419 = data
  request.r(i419[0], i419[1], 0, i418, 'shader')
  i418.pass = i419[2]
  return i418
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue"] = function (request, data, root) {
  var i422 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue' )
  var i423 = data
  i422.name = i423[0]
  i422.type = i423[1]
  i422.value = new pc.Vec4( i423[2], i423[3], i423[4], i423[5] )
  i422.textureValue = i423[6]
  i422.shaderPropertyFlag = i423[7]
  return i422
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources"] = function (request, data, root) {
  var i424 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources' )
  var i425 = data
  var i427 = i425[0]
  var i426 = []
  for(var i = 0; i < i427.length; i += 1) {
    i426.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.Resources+File', i427[i + 0]) );
  }
  i424.files = i426
  i424.componentToPrefabIds = i425[1]
  return i424
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Resources+File"] = function (request, data, root) {
  var i430 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Resources+File' )
  var i431 = data
  i430.path = i431[0]
  request.r(i431[1], i431[2], 0, i430, 'unityObject')
  return i430
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings"] = function (request, data, root) {
  var i432 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings' )
  var i433 = data
  var i435 = i433[0]
  var i434 = []
  for(var i = 0; i < i435.length; i += 1) {
    i434.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder', i435[i + 0]) );
  }
  i432.scriptsExecutionOrder = i434
  var i437 = i433[1]
  var i436 = []
  for(var i = 0; i < i437.length; i += 1) {
    i436.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer', i437[i + 0]) );
  }
  i432.sortingLayers = i436
  var i439 = i433[2]
  var i438 = []
  for(var i = 0; i < i439.length; i += 1) {
    i438.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer', i439[i + 0]) );
  }
  i432.cullingLayers = i438
  i432.timeSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings', i433[3], i432.timeSettings)
  i432.physicsSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings', i433[4], i432.physicsSettings)
  i432.physics2DSettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings', i433[5], i432.physics2DSettings)
  i432.qualitySettings = request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i433[6], i432.qualitySettings)
  i432.enableRealtimeShadows = !!i433[7]
  i432.enableAutoInstancing = !!i433[8]
  i432.enableStaticBatching = !!i433[9]
  i432.enableDynamicBatching = !!i433[10]
  i432.usePreservativeDynamicBatching = !!i433[11]
  i432.lightmapEncodingQuality = i433[12]
  i432.desiredColorSpace = i433[13]
  var i441 = i433[14]
  var i440 = []
  for(var i = 0; i < i441.length; i += 1) {
    i440.push( i441[i + 0] );
  }
  i432.allTags = i440
  return i432
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder"] = function (request, data, root) {
  var i444 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder' )
  var i445 = data
  i444.name = i445[0]
  i444.value = i445[1]
  return i444
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer"] = function (request, data, root) {
  var i448 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer' )
  var i449 = data
  i448.id = i449[0]
  i448.name = i449[1]
  i448.value = i449[2]
  return i448
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer"] = function (request, data, root) {
  var i452 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer' )
  var i453 = data
  i452.id = i453[0]
  i452.name = i453[1]
  return i452
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings"] = function (request, data, root) {
  var i454 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings' )
  var i455 = data
  i454.fixedDeltaTime = i455[0]
  i454.maximumDeltaTime = i455[1]
  i454.timeScale = i455[2]
  i454.maximumParticleTimestep = i455[3]
  return i454
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings"] = function (request, data, root) {
  var i456 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings' )
  var i457 = data
  i456.gravity = new pc.Vec3( i457[0], i457[1], i457[2] )
  i456.defaultSolverIterations = i457[3]
  i456.bounceThreshold = i457[4]
  i456.autoSyncTransforms = !!i457[5]
  i456.autoSimulation = !!i457[6]
  var i459 = i457[7]
  var i458 = []
  for(var i = 0; i < i459.length; i += 1) {
    i458.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask', i459[i + 0]) );
  }
  i456.collisionMatrix = i458
  return i456
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask"] = function (request, data, root) {
  var i462 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask' )
  var i463 = data
  i462.enabled = !!i463[0]
  i462.layerId = i463[1]
  i462.otherLayerId = i463[2]
  return i462
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings"] = function (request, data, root) {
  var i464 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings' )
  var i465 = data
  request.r(i465[0], i465[1], 0, i464, 'material')
  i464.gravity = new pc.Vec2( i465[2], i465[3] )
  i464.positionIterations = i465[4]
  i464.velocityIterations = i465[5]
  i464.velocityThreshold = i465[6]
  i464.maxLinearCorrection = i465[7]
  i464.maxAngularCorrection = i465[8]
  i464.maxTranslationSpeed = i465[9]
  i464.maxRotationSpeed = i465[10]
  i464.baumgarteScale = i465[11]
  i464.baumgarteTOIScale = i465[12]
  i464.timeToSleep = i465[13]
  i464.linearSleepTolerance = i465[14]
  i464.angularSleepTolerance = i465[15]
  i464.defaultContactOffset = i465[16]
  i464.autoSimulation = !!i465[17]
  i464.queriesHitTriggers = !!i465[18]
  i464.queriesStartInColliders = !!i465[19]
  i464.callbacksOnDisable = !!i465[20]
  i464.reuseCollisionCallbacks = !!i465[21]
  i464.autoSyncTransforms = !!i465[22]
  var i467 = i465[23]
  var i466 = []
  for(var i = 0; i < i467.length; i += 1) {
    i466.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask', i467[i + 0]) );
  }
  i464.collisionMatrix = i466
  return i464
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask"] = function (request, data, root) {
  var i470 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask' )
  var i471 = data
  i470.enabled = !!i471[0]
  i470.layerId = i471[1]
  i470.otherLayerId = i471[2]
  return i470
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.QualitySettings"] = function (request, data, root) {
  var i472 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.QualitySettings' )
  var i473 = data
  var i475 = i473[0]
  var i474 = []
  for(var i = 0; i < i475.length; i += 1) {
    i474.push( request.d('Luna.Unity.DTO.UnityEngine.Assets.QualitySettings', i475[i + 0]) );
  }
  i472.qualityLevels = i474
  var i477 = i473[1]
  var i476 = []
  for(var i = 0; i < i477.length; i += 1) {
    i476.push( i477[i + 0] );
  }
  i472.names = i476
  i472.shadows = i473[2]
  i472.anisotropicFiltering = i473[3]
  i472.antiAliasing = i473[4]
  i472.lodBias = i473[5]
  i472.shadowCascades = i473[6]
  i472.shadowDistance = i473[7]
  i472.shadowmaskMode = i473[8]
  i472.shadowProjection = i473[9]
  i472.shadowResolution = i473[10]
  i472.softParticles = !!i473[11]
  i472.softVegetation = !!i473[12]
  i472.activeColorSpace = i473[13]
  i472.desiredColorSpace = i473[14]
  i472.masterTextureLimit = i473[15]
  i472.maxQueuedFrames = i473[16]
  i472.particleRaycastBudget = i473[17]
  i472.pixelLightCount = i473[18]
  i472.realtimeReflectionProbes = !!i473[19]
  i472.shadowCascade2Split = i473[20]
  i472.shadowCascade4Split = new pc.Vec3( i473[21], i473[22], i473[23] )
  i472.streamingMipmapsActive = !!i473[24]
  i472.vSyncCount = i473[25]
  i472.asyncUploadBufferSize = i473[26]
  i472.asyncUploadTimeSlice = i473[27]
  i472.billboardsFaceCameraPosition = !!i473[28]
  i472.shadowNearPlaneOffset = i473[29]
  i472.streamingMipmapsMemoryBudget = i473[30]
  i472.maximumLODLevel = i473[31]
  i472.streamingMipmapsAddAllCameras = !!i473[32]
  i472.streamingMipmapsMaxLevelReduction = i473[33]
  i472.streamingMipmapsRenderersPerFrame = i473[34]
  i472.resolutionScalingFixedDPIFactor = i473[35]
  i472.streamingMipmapsMaxFileIORequests = i473[36]
  i472.currentQualityLevel = i473[37]
  return i472
}

Deserializers["Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame"] = function (request, data, root) {
  var i482 = root || request.c( 'Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame' )
  var i483 = data
  i482.weight = i483[0]
  i482.vertices = i483[1]
  i482.normals = i483[2]
  i482.tangents = i483[3]
  return i482
}

Deserializers.fields = {"Luna.Unity.DTO.UnityEngine.Assets.Mesh":{"name":0,"halfPrecision":1,"useSimplification":2,"useUInt32IndexFormat":3,"vertexCount":4,"aabb":5,"streams":6,"vertices":7,"subMeshes":8,"bindposes":9,"blendShapes":10},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+SubMesh":{"triangles":0},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShape":{"name":0,"frames":1},"Luna.Unity.DTO.UnityEngine.Assets.Material":{"name":0,"shader":1,"renderQueue":3,"enableInstancing":4,"floatParameters":5,"colorParameters":6,"vectorParameters":7,"textureParameters":8,"materialFlags":9},"Luna.Unity.DTO.UnityEngine.Assets.Material+FloatParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+ColorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+VectorParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+TextureParameter":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Material+MaterialFlag":{"name":0,"enabled":1},"Luna.Unity.DTO.UnityEngine.Textures.Texture2D":{"name":0,"width":1,"height":2,"mipmapCount":3,"anisoLevel":4,"filterMode":5,"hdr":6,"format":7,"wrapMode":8,"alphaIsTransparency":9,"alphaSource":10,"graphicsFormat":11,"sRGBTexture":12,"desiredColorSpace":13,"wrapU":14,"wrapV":15},"Luna.Unity.DTO.UnityEngine.Textures.Cubemap":{"name":0,"atlasId":1,"mipmapCount":2,"hdr":3,"size":4,"anisoLevel":5,"filterMode":6,"rects":7,"wrapU":8,"wrapV":9},"Luna.Unity.DTO.UnityEngine.Scene.Scene":{"name":0,"index":1,"startup":2},"Luna.Unity.DTO.UnityEngine.Components.Camera":{"aspect":0,"orthographic":1,"orthographicSize":2,"backgroundColor":3,"nearClipPlane":7,"farClipPlane":8,"fieldOfView":9,"depth":10,"clearFlags":11,"cullingMask":12,"rect":13,"targetTexture":14,"usePhysicalProperties":16,"focalLength":17,"sensorSize":18,"lensShift":20,"gateFit":22,"commandBufferCount":23,"cameraType":24,"enabled":25},"Luna.Unity.DTO.UnityEngine.Scene.GameObject":{"name":0,"tagId":1,"enabled":2,"isStatic":3,"layer":4},"Luna.Unity.DTO.UnityEngine.Components.Light":{"type":0,"color":1,"cullingMask":5,"intensity":6,"range":7,"spotAngle":8,"shadows":9,"shadowNormalBias":10,"shadowBias":11,"shadowStrength":12,"shadowResolution":13,"lightmapBakeType":14,"renderMode":15,"cookie":16,"cookieSize":18,"shadowNearPlane":19,"occlusionMaskChannel":20,"isBaked":21,"mixedLightingMode":22,"enabled":23},"Luna.Unity.DTO.UnityEngine.Components.MeshFilter":{"sharedMesh":0},"Luna.Unity.DTO.UnityEngine.Components.MeshRenderer":{"additionalVertexStreams":0,"enabled":2,"sharedMaterial":3,"sharedMaterials":5,"receiveShadows":6,"shadowCastingMode":7,"sortingLayerID":8,"sortingOrder":9,"lightmapIndex":10,"lightmapSceneIndex":11,"lightmapScaleOffset":12,"lightProbeUsage":16,"reflectionProbeUsage":17},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings":{"ambientIntensity":0,"reflectionIntensity":1,"ambientMode":2,"ambientLight":3,"ambientSkyColor":7,"ambientGroundColor":11,"ambientEquatorColor":15,"fogColor":19,"fogEndDistance":23,"fogStartDistance":24,"fogDensity":25,"fog":26,"skybox":27,"fogMode":29,"lightmaps":30,"lightProbes":31,"lightmapsMode":32,"mixedBakeMode":33,"environmentLightingMode":34,"ambientProbe":35,"customReflection":36,"defaultReflection":38,"defaultReflectionMode":40,"defaultReflectionResolution":41,"sunLightObjectId":42,"pixelLightCount":43,"defaultReflectionHDR":44,"hasLightDataAsset":45,"hasManualGenerate":46},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+Lightmap":{"lightmapColor":0,"lightmapDirection":2,"shadowMask":4},"Luna.Unity.DTO.UnityEngine.Assets.RenderSettings+LightProbes":{"bakedProbes":0,"positions":1,"hullRays":2,"tetrahedra":3,"neighbours":4,"matrices":5},"Luna.Unity.DTO.UnityEngine.Assets.Shader":{"ShaderCompilationErrors":0,"name":1,"guid":2,"shaderDefinedKeywords":3,"passes":4,"usePasses":5,"defaultParameterValues":6,"unityFallbackShader":7,"readDepth":9,"hasDepthOnlyPass":10,"isCreatedByShaderGraph":11,"disableBatching":12,"compiled":13},"Luna.Unity.DTO.UnityEngine.Assets.Shader+ShaderCompilationError":{"shaderName":0,"errorMessage":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass":{"id":0,"subShaderIndex":1,"name":2,"passType":3,"grabPassTextureName":4,"usePass":5,"zTest":6,"zWrite":7,"culling":8,"blending":9,"alphaBlending":10,"colorWriteMask":11,"offsetUnits":12,"offsetFactor":13,"stencilRef":14,"stencilReadMask":15,"stencilWriteMask":16,"stencilOp":17,"stencilOpFront":18,"stencilOpBack":19,"tags":20,"passDefinedKeywords":21,"passDefinedKeywordGroups":22,"variants":23,"excludedVariants":24,"hasDepthReader":25},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Value":{"val":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Blending":{"src":0,"dst":1,"op":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+StencilOp":{"pass":0,"fail":1,"zFail":2,"comp":3},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Tag":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+KeywordGroup":{"keywords":0,"hasDiscard":1},"Luna.Unity.DTO.UnityEngine.Assets.Shader+Pass+Variant":{"passId":0,"subShaderIndex":1,"keywords":2,"vertexProgram":3,"fragmentProgram":4,"exportedForWebGl2":5,"readDepth":6},"Luna.Unity.DTO.UnityEngine.Assets.Shader+UsePass":{"shader":0,"pass":2},"Luna.Unity.DTO.UnityEngine.Assets.Shader+DefaultParameterValue":{"name":0,"type":1,"value":2,"textureValue":6,"shaderPropertyFlag":7},"Luna.Unity.DTO.UnityEngine.Assets.Resources":{"files":0,"componentToPrefabIds":1},"Luna.Unity.DTO.UnityEngine.Assets.Resources+File":{"path":0,"unityObject":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings":{"scriptsExecutionOrder":0,"sortingLayers":1,"cullingLayers":2,"timeSettings":3,"physicsSettings":4,"physics2DSettings":5,"qualitySettings":6,"enableRealtimeShadows":7,"enableAutoInstancing":8,"enableStaticBatching":9,"enableDynamicBatching":10,"usePreservativeDynamicBatching":11,"lightmapEncodingQuality":12,"desiredColorSpace":13,"allTags":14},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+ScriptsExecutionOrder":{"name":0,"value":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+SortingLayer":{"id":0,"name":1,"value":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+CullingLayer":{"id":0,"name":1},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+TimeSettings":{"fixedDeltaTime":0,"maximumDeltaTime":1,"timeScale":2,"maximumParticleTimestep":3},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings":{"gravity":0,"defaultSolverIterations":3,"bounceThreshold":4,"autoSyncTransforms":5,"autoSimulation":6,"collisionMatrix":7},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+PhysicsSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings":{"material":0,"gravity":2,"positionIterations":4,"velocityIterations":5,"velocityThreshold":6,"maxLinearCorrection":7,"maxAngularCorrection":8,"maxTranslationSpeed":9,"maxRotationSpeed":10,"baumgarteScale":11,"baumgarteTOIScale":12,"timeToSleep":13,"linearSleepTolerance":14,"angularSleepTolerance":15,"defaultContactOffset":16,"autoSimulation":17,"queriesHitTriggers":18,"queriesStartInColliders":19,"callbacksOnDisable":20,"reuseCollisionCallbacks":21,"autoSyncTransforms":22,"collisionMatrix":23},"Luna.Unity.DTO.UnityEngine.Assets.ProjectSettings+Physics2DSettings+CollisionMask":{"enabled":0,"layerId":1,"otherLayerId":2},"Luna.Unity.DTO.UnityEngine.Assets.QualitySettings":{"qualityLevels":0,"names":1,"shadows":2,"anisotropicFiltering":3,"antiAliasing":4,"lodBias":5,"shadowCascades":6,"shadowDistance":7,"shadowmaskMode":8,"shadowProjection":9,"shadowResolution":10,"softParticles":11,"softVegetation":12,"activeColorSpace":13,"desiredColorSpace":14,"masterTextureLimit":15,"maxQueuedFrames":16,"particleRaycastBudget":17,"pixelLightCount":18,"realtimeReflectionProbes":19,"shadowCascade2Split":20,"shadowCascade4Split":21,"streamingMipmapsActive":24,"vSyncCount":25,"asyncUploadBufferSize":26,"asyncUploadTimeSlice":27,"billboardsFaceCameraPosition":28,"shadowNearPlaneOffset":29,"streamingMipmapsMemoryBudget":30,"maximumLODLevel":31,"streamingMipmapsAddAllCameras":32,"streamingMipmapsMaxLevelReduction":33,"streamingMipmapsRenderersPerFrame":34,"resolutionScalingFixedDPIFactor":35,"streamingMipmapsMaxFileIORequests":36,"currentQualityLevel":37},"Luna.Unity.DTO.UnityEngine.Assets.Mesh+BlendShapeFrame":{"weight":0,"vertices":1,"normals":2,"tangents":3}}

Deserializers.requiredComponents = {"13":[14],"15":[14],"16":[14],"17":[14],"18":[14],"19":[14],"20":[21],"22":[2],"23":[24],"25":[24],"26":[24],"27":[24],"28":[24],"29":[24],"30":[24],"31":[32],"33":[32],"34":[32],"35":[32],"36":[32],"37":[32],"38":[32],"39":[32],"40":[32],"41":[32],"42":[32],"43":[32],"44":[32],"45":[2],"46":[7],"47":[48],"49":[48],"50":[51],"52":[51],"53":[2],"54":[2],"55":[4],"56":[57],"58":[51],"59":[51],"60":[50],"61":[62,51],"63":[51],"64":[50],"65":[51],"66":[51],"67":[51],"68":[51],"69":[51],"70":[51],"71":[51],"72":[51],"73":[51],"74":[62,51],"75":[51],"76":[51],"77":[51],"78":[51],"79":[62,51],"80":[51],"81":[82],"83":[82],"84":[82],"85":[82],"86":[2],"87":[2],"88":[57],"89":[51],"90":[7,51],"91":[51,62],"92":[51],"93":[62,51],"94":[7],"95":[62,51],"96":[51],"97":[57]}

Deserializers.types = ["UnityEngine.Shader","UnityEngine.Texture2D","UnityEngine.Camera","UnityEngine.AudioListener","UnityEngine.Light","UnityEngine.MeshFilter","UnityEngine.Mesh","UnityEngine.MeshRenderer","UnityEngine.Material","UnityEngine.MonoBehaviour","NewPlaneBehaviour.Plane","UnityEngine.Transform","UnityEngine.Cubemap","UnityEngine.AudioLowPassFilter","UnityEngine.AudioBehaviour","UnityEngine.AudioHighPassFilter","UnityEngine.AudioReverbFilter","UnityEngine.AudioDistortionFilter","UnityEngine.AudioEchoFilter","UnityEngine.AudioChorusFilter","UnityEngine.Cloth","UnityEngine.SkinnedMeshRenderer","UnityEngine.FlareLayer","UnityEngine.ConstantForce","UnityEngine.Rigidbody","UnityEngine.Joint","UnityEngine.HingeJoint","UnityEngine.SpringJoint","UnityEngine.FixedJoint","UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.CompositeCollider2D","UnityEngine.Rigidbody2D","UnityEngine.Joint2D","UnityEngine.AnchoredJoint2D","UnityEngine.SpringJoint2D","UnityEngine.DistanceJoint2D","UnityEngine.FrictionJoint2D","UnityEngine.HingeJoint2D","UnityEngine.RelativeJoint2D","UnityEngine.SliderJoint2D","UnityEngine.TargetJoint2D","UnityEngine.FixedJoint2D","UnityEngine.WheelJoint2D","UnityEngine.ConstantForce2D","UnityEngine.StreamingController","UnityEngine.TextMesh","UnityEngine.Tilemaps.TilemapRenderer","UnityEngine.Tilemaps.Tilemap","UnityEngine.Tilemaps.TilemapCollider2D","UnityEngine.Canvas","UnityEngine.RectTransform","UnityEngine.Rendering.UI.UIFoldout","UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera","UnityEngine.Rendering.Universal.UniversalAdditionalCameraData","UnityEngine.Rendering.Universal.UniversalAdditionalLightData","Unity.VisualScripting.SceneVariables","Unity.VisualScripting.Variables","UnityEngine.UI.Dropdown","UnityEngine.UI.Graphic","UnityEngine.UI.GraphicRaycaster","UnityEngine.UI.Image","UnityEngine.CanvasRenderer","UnityEngine.UI.AspectRatioFitter","UnityEngine.UI.CanvasScaler","UnityEngine.UI.ContentSizeFitter","UnityEngine.UI.GridLayoutGroup","UnityEngine.UI.HorizontalLayoutGroup","UnityEngine.UI.HorizontalOrVerticalLayoutGroup","UnityEngine.UI.LayoutElement","UnityEngine.UI.LayoutGroup","UnityEngine.UI.VerticalLayoutGroup","UnityEngine.UI.Mask","UnityEngine.UI.MaskableGraphic","UnityEngine.UI.RawImage","UnityEngine.UI.RectMask2D","UnityEngine.UI.Scrollbar","UnityEngine.UI.ScrollRect","UnityEngine.UI.Slider","UnityEngine.UI.Text","UnityEngine.UI.Toggle","UnityEngine.EventSystems.BaseInputModule","UnityEngine.EventSystems.EventSystem","UnityEngine.EventSystems.PointerInputModule","UnityEngine.EventSystems.StandaloneInputModule","UnityEngine.EventSystems.TouchInputModule","UnityEngine.EventSystems.Physics2DRaycaster","UnityEngine.EventSystems.PhysicsRaycaster","Unity.VisualScripting.ScriptMachine","TMPro.TextContainer","TMPro.TextMeshPro","TMPro.TextMeshProUGUI","TMPro.TMP_Dropdown","TMPro.TMP_SelectionCaret","TMPro.TMP_SubMesh","TMPro.TMP_SubMeshUI","TMPro.TMP_Text","Unity.VisualScripting.StateMachine"]

Deserializers.unityVersion = "2022.3.62f2";

Deserializers.productName = "Epic Plane Evolution - Playable";

Deserializers.lunaInitializationTime = "11/02/2026 18:27:18";

Deserializers.lunaDaysRunning = "0.9";

Deserializers.lunaVersion = "7.2.0";

Deserializers.lunaSHA = "ea08d29afe2968efcb8d91d5624f033c6485cc68";

Deserializers.creativeName = "Empty-Test";

Deserializers.lunaAppID = "44011";

Deserializers.projectId = "35cb19000dcb5644fbd0d939d5b46bce";

Deserializers.packagesInfo = "com.unity.render-pipelines.universal: 14.0.12\ncom.unity.textmeshpro: 3.0.7\ncom.unity.timeline: 1.7.7\ncom.unity.ugui: 1.0.0";

Deserializers.externalJsLibraries = "";

Deserializers.androidLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.androidLink?window.$environment.packageConfig.androidLink:'Empty';

Deserializers.iosLink = ( typeof window !== "undefined")&&window.$environment.packageConfig.iosLink?window.$environment.packageConfig.iosLink:'Empty';

Deserializers.base64Enabled = "False";

Deserializers.minifyEnabled = "True";

Deserializers.isForceUncompressed = "False";

Deserializers.isAntiAliasingEnabled = "False";

Deserializers.isRuntimeAnalysisEnabledForCode = "False";

Deserializers.runtimeAnalysisExcludedClassesCount = "1840";

Deserializers.runtimeAnalysisExcludedMethodsCount = "3127";

Deserializers.runtimeAnalysisExcludedModules = "physics2d, particle-system, reflection, prefabs, mecanim-wasm";

Deserializers.isRuntimeAnalysisEnabledForShaders = "True";

Deserializers.isRealtimeShadowsEnabled = "False";

Deserializers.isLunaCompilerV2Used = "False";

Deserializers.companyName = "DefaultCompany";

Deserializers.buildPlatform = "WebGL";

Deserializers.applicationIdentifier = "com.DefaultCompany.Epic-Plane-Evolution---Playable";

Deserializers.disableAntiAliasing = true;

Deserializers.graphicsConstraint = 24;

Deserializers.linearColorSpace = true;

Deserializers.buildID = "7d37cbc4-af70-4ba8-85d3-86b4293ed3fa";

Deserializers.runtimeInitializeOnLoadInfos = [[["UnityEngine","Rendering","DebugUpdater","RuntimeInit"],["UnityEngine","Experimental","Rendering","ScriptableRuntimeReflectionSystemSettings","ScriptingDirtyReflectionSystemInstance"]],[["Unity","VisualScripting","RuntimeVSUsageUtility","RuntimeInitializeOnLoadBeforeSceneLoad"]],[],[["UnityEngine","Experimental","Rendering","XRSystem","XRSystemInit"]],[]];

Deserializers.typeNameToIdMap = function(){ var i = 0; return Deserializers.types.reduce( function( res, item ) { res[ item ] = i++; return res; }, {} ) }()

