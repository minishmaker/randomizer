.thumb
ldr	r3,=#0x801D668
mov	lr,r3
.short	0xF800
ldr	r1,=#0x3000F50
mov	r0,#1
strh	r0,[r1,#0x0E]

@check if file select
push	{r0-r7}
ldr	r0,=#0x3001002
ldrb	r0,[r0]
cmp	r0,#1
bne	end
@draw the tiles
ldr	r4,=#0x811DBD5
mov	r5,#0
ldr	r6,=#0x2034CB6
ldr	r7,hashIconsTable
iconsLoop:
ldrb	r0,[r4,r5]
mov	r1,#0x3F
and	r0,r1
lsl	r0,#1
ldrh	r0,[r7,r0]
lsl	r1,r5,#2
orr	r0,r1
add	r0,#0x40
strh	r0,[r6]
add	r0,#1
strh	r0,[r6,#2]
add	r0,#1
add	r6,#0x40
strh	r0,[r6]
add	r0,#1
strh	r0,[r6,#2]
sub	r6,#0x40
add	r6,#4
add	r5,#1
cmp	r5,#5
bne	np
ldr	r0,=#0x0C5F
mov	r1,#0x40
strh	r0,[r6,r1]
add	r6,#2
np:
cmp	r5,#7
blo	iconsLoop

end:
pop	{r0-r7}
pop	{pc}
.align
.ltorg
hashIconsTable:
