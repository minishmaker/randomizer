.thumb
push	{lr}

@check if we ever entered through the main entrance
mov	r0,#0x58
ldr	r3,=#0x807C654
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	sickGregal

mainEntrance:
@check if we already saved gregal
mov	r0,#0x63
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	zombieGregal

@check if gregal already gave us his first item
ldr	r0,=#0x265
ldr	r3,=#0x807C654
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	zombieGregal

@gregal is cured
ldr	r0,=#0x804EDC4
ldr	r0,[r0]
b	End

@gregal is sick like normal
sickGregal:
ldr	r0,=#0x804EDAC
ldr	r0,[r0]
b	End

@gregal is sick instead of dead, staircase is not blocked
zombieGregal:
ldr	r0,new
b	End

End:
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
pop	{pc}
.align
.ltorg
new:
