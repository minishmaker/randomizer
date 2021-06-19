.equ progressiveTraps, extraIconTable+4
.thumb
ldrb	r2,[r0,#0x0A]
cmp	r2, #0x1B
beq	maybeTrap
cmp		r2,#0x05
bne		notExtra
ldrh	r2,[r0,#0x12]
ldr		r3,=#0x141
cmp		r2,r3
bne		notExtra
ldrb	r1,[r0,#0x0B]
ldr		r2,extraIconTable
b		end

notExtra:
ldrh	r3,[r0,#0x12]
lsl		r3,#4
ldr		r2,=#0x8004434
ldr		r2,[r2]
ldr		r2,[r2,r3]

end:
lsl		r1,#2
ldr		r1,[r2,r1]
ldr		r3,=#0x8004309
bx		r3

maybeTrap:
ldrh	r2,[r0,#0x12]
ldr		r3,=#0x141
cmp		r2,r3
bne		notExtra
ldr	r2,progressiveTraps
ldrb	r2, [r2, r1]
cmp	r2, #0xFF
beq	notExtra
mov	r1, r2
ldr		r2,extraIconTable
b	end

.align
.ltorg
extraIconTable:
@POIN extraIconTable
@POIN progressiveTraps
